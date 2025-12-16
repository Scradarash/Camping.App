using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Camping.App.ViewModels;

public partial class ReserveringsoverzichtViewModel : ObservableObject
{
    private readonly IReservatieDataService _reservatieDataService;

    // gebruiken de Service voor de logica (ipv de Repository)
    private readonly IAccommodatieService _accommodatieService;
    private readonly IReserveringService _reserveringService;

    // Prijzen definities (hardcoded voor nu, kan later uit DB/Service)
    private const decimal PRIJS_BASIS_PER_NACHT = 20.00m;
    private const decimal PRIJS_STROOM = 4.99m;
    private const decimal PRIJS_WATER = 0.00m;

    [ObservableProperty]
    private string periodDescription;

    [ObservableProperty]
    private string fieldName;

    // Nieuwe property om de naam van de geselecteerde staanplaats weer te geven in de View
    [ObservableProperty]
    private string staanplaatsNaam;

    [ObservableProperty]
    private Accommodatie selectedAccommodatie;

    [ObservableProperty]
    private string naam;

    [ObservableProperty]
    private string naamFoutmelding;

    [ObservableProperty]
    private bool isNaamFoutZichtbaar;

    [ObservableProperty]
    private DateTime? geboortedatum;

    [ObservableProperty]
    private string geboortedatumFoutmelding;

    [ObservableProperty]
    private bool isGeboortedatumFoutZichtbaar;

    [ObservableProperty]
    private string emailadres;

    [ObservableProperty]
    private string emailadresFoutmelding;

    [ObservableProperty]
    private bool isEmailadresFoutZichtbaar;

    [ObservableProperty]
    private string telefoonnummer;

    [ObservableProperty]
    private string telefoonnummerFoutmelding;

    [ObservableProperty]
    private bool isTelefoonnummerFoutZichtbaar;

    //Properties om te bepalen of checkbox zichtbaar is
    [ObservableProperty]
    private bool isStroomMogelijk;

    [ObservableProperty]
    private bool isWaterMogelijk;

    // [NotifyPropertyChangedFor] zorgt dat de TotaalPrijsTekst automatisch update als je klikt!
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotaalPrijsTekst))]
    private bool kiestStroom;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotaalPrijsTekst))]
    private bool kiestWater;

    public string StroomPrijsTekst => $"Stroom €{PRIJS_STROOM},- per nacht";
    public string WaterPrijsTekst => $"Water €{PRIJS_WATER},-";

    public string TotaalPrijsTekst => $"Totaalprijs: €{BerekenEindTotaal():F2}";

    public ObservableCollection<Accommodatie> Accommodaties { get; } = new();

    public ReserveringsoverzichtViewModel(IReservatieDataService reservatieDataService, IAccommodatieService accommodatieService, IReserveringService reserveringService)
    {
        _reservatieDataService = reservatieDataService;
        _accommodatieService = accommodatieService;
        _reserveringService = reserveringService;

        LoadData();
    }

    private void LoadData()
    {
        // Veiligheidscheck: als er geen veld is, stop dan (voorkomt crashes)
        if (_reservatieDataService.SelectedVeld == null || _reservatieDataService.StartDate == null)
        {
            return;
        }

        FieldName = _reservatieDataService.SelectedVeld.Name;
        PeriodDescription = $"{_reservatieDataService.StartDate:dd-MM-yyyy} tot {_reservatieDataService.EndDate:dd-MM-yyyy}";

        // Gegevens ophalen van de geselecteerde staanplaats uit de service
        var gekozenPlek = _reservatieDataService.SelectedStaanplaats;

        if (gekozenPlek != null)
        {
            // Als er een plek gekozen is, tonen we het nummer en type
            StaanplaatsNaam = $"Plaats: {gekozenPlek.id} ({gekozenPlek.AccommodatieType})";

            // Checken wat de plek daadwerkelijk heeft (uit DB data)
            IsStroomMogelijk = gekozenPlek.HeeftStroom;
            IsWaterMogelijk = gekozenPlek.HeeftWater;

            // Resetten van keuzes (standaard uit)
            KiestStroom = _reservatieDataService.KiestStroom && IsStroomMogelijk;
            KiestWater = _reservatieDataService.KiestWater && IsWaterMogelijk;
        }
        else
        {
            StaanplaatsNaam = "Geen specifieke plaats gekozen";

            IsStroomMogelijk = false;
            IsWaterMogelijk = false;

            KiestStroom = false;
            KiestWater = false;
        }

        Accommodaties.Clear();

        // We vragen aan de service: wat mag er staan op dit veld"
        var gefilterdeLijst = _accommodatieService.GetGeschikteAccommodaties(_reservatieDataService.SelectedVeld);

        foreach (var acc in gefilterdeLijst)
        {
            // In het reserveringsoverzicht alleen accommodaties tonen die overeenkomen met de gekozen staanplaats
            if (gekozenPlek != null)
            {
                if (acc.Name.Contains(gekozenPlek.AccommodatieType, StringComparison.OrdinalIgnoreCase)
                    || gekozenPlek.AccommodatieType.Contains(acc.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Accommodaties.Add(acc);
                }
            }
            else
            {
                Accommodaties.Add(acc);
            }
        }

        if (Accommodaties.Count > 0)
        {
            SelectedAccommodatie = Accommodaties[0];
        }

        Naam = _reservatieDataService.Naam;

        OnPropertyChanged(nameof(TotaalPrijsTekst));
    }

    private decimal BerekenEindTotaal()
    {
        if (_reservatieDataService.StartDate == null || _reservatieDataService.EndDate == null)
            return 0.00m;

        int nachten = (_reservatieDataService.EndDate.Value - _reservatieDataService.StartDate.Value).Days;
        if (nachten < 1) nachten = 1;

        decimal totaal = nachten * PRIJS_BASIS_PER_NACHT;

        // <--- Logica: Tel alleen op als aangevinkt EN mogelijk
        if (KiestStroom && IsStroomMogelijk)
        {
            totaal += (nachten * PRIJS_STROOM);
        }

        if (KiestWater && IsWaterMogelijk)
        {
            totaal += (nachten * PRIJS_WATER);
        }

        return totaal;
    }

    [RelayCommand]
    private async Task BevestigAccommodatie()
    {
        var naamOk = HasValidNaam();
        var geboortedatumOk = HasValidGeboortedatum();
        var emailOk = HasValidEmailadres();
        var telefoonOk = HasValidTelefoonnummer();

        if (!naamOk || !geboortedatumOk || !emailOk || !telefoonOk)
        {
            return;
        }

        if (SelectedAccommodatie == null)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Kies een accommodatie type.", "OK");
            return;
        }

        if (!_reservatieDataService.StartDate.HasValue || !_reservatieDataService.EndDate.HasValue)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Er is nog geen periode geselecteerd.", "OK");
            return;
        }

        if (_reservatieDataService.SelectedVeld == null)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Er is geen veld geselecteerd.", "OK");
            return;
        }

        if (_reservatieDataService.SelectedStaanplaats == null)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Er is geen staanplaats geselecteerd.", "OK");
            return;
        }

        try
        {
            _reservatieDataService.Naam = Naam;
            _reservatieDataService.Geboortedatum = Geboortedatum;
            _reservatieDataService.Emailadres = Emailadres;
            _reservatieDataService.Telefoonnummer = Telefoonnummer;
            _reservatieDataService.SelectedAccommodatie = SelectedAccommodatie;

            _reservatieDataService.KiestStroom = KiestStroom;
            _reservatieDataService.KiestWater = KiestWater;

            decimal definitievePrijs = BerekenEindTotaal();

            await _reserveringService.MaakReserveringAsync(
                _reservatieDataService.StartDate.Value,
                _reservatieDataService.EndDate.Value,
                _reservatieDataService.SelectedVeld,
                _reservatieDataService.SelectedStaanplaats,
                SelectedAccommodatie,
                kiestStroom,
                KiestWater,
                definitievePrijs);

            await Application.Current.MainPage.DisplayAlert(
                "Reservering voltooid",
                "De reservering is succesvol opgeslagen.",
                "OK");

            await Shell.Current.GoToAsync("//PlattegrondView");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Fout bij opslaan",
                $"Er ging iets mis bij het opslaan van de reservering:\n{ex.Message}",
                "OK");
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    private bool HasValidNaam()
    {
        if (string.IsNullOrWhiteSpace(Naam))
        {
            NaamFoutmelding = "Naam is verplicht.";
            IsNaamFoutZichtbaar = true;
            return false;
        }

        var trimmed = Naam.Trim();

        if (trimmed.Length < 2)
        {
            NaamFoutmelding = "Naam moet minimaal 2 tekens bevatten.";
            IsNaamFoutZichtbaar = true;
            return false;
        }

        if (trimmed.Length > 25)
        {
            NaamFoutmelding = "Naam mag maximaal 25 tekens bevatten.";
            IsNaamFoutZichtbaar = true;
            return false;
        }

        var regex = new Regex(@"^[a-zA-ZÀ-ÿ\s\-']+$");

        if (!regex.IsMatch(trimmed))
        {
            NaamFoutmelding = "Naam bevat ongeldige tekens.";
            IsNaamFoutZichtbaar = true;
            return false;
        }

        NaamFoutmelding = string.Empty;
        IsNaamFoutZichtbaar = false;
        return true;
    }

    private bool HasValidGeboortedatum()
    {
        if (Geboortedatum == null)
        {
            GeboortedatumFoutmelding = "Geboortedatum is verplicht.";
            IsGeboortedatumFoutZichtbaar = true;
            return false;
        }

        var vandaag = DateTime.Today;
        var leeftijd = vandaag.Year - Geboortedatum.Value.Year;
        if (Geboortedatum.Value.Date > vandaag.AddYears(-leeftijd)) leeftijd--;

        if (leeftijd < 18)
        {
            GeboortedatumFoutmelding = "De hoofdboeker moet minimaal 18 jaar zijn.";
            IsGeboortedatumFoutZichtbaar = true;
            return false;
        }

        if (leeftijd > 120)
        {
            GeboortedatumFoutmelding = "Leeftijd boven 120 jaar is niet toegestaan.";
            IsGeboortedatumFoutZichtbaar = true;
            return false;
        }

        GeboortedatumFoutmelding = string.Empty;
        IsGeboortedatumFoutZichtbaar = false;
        return true;
    }

    private bool HasValidEmailadres()
    {
        if (string.IsNullOrWhiteSpace(Emailadres))
        {
            EmailadresFoutmelding = "E-mailadres is verplicht.";
            IsEmailadresFoutZichtbaar = true;
            return false;
        }

        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(Emailadres, pattern, RegexOptions.IgnoreCase))
        {
            EmailadresFoutmelding = "E-mailadres structuur klopt niet.";
            IsEmailadresFoutZichtbaar = true;
            return false;
        }

        if (Regex.IsMatch(Emailadres, @"[^a-zA-Z0-9@\.\-_]", RegexOptions.IgnoreCase))
        {
            EmailadresFoutmelding = "E-mailadres bevat niet toegestane karakters.";
            IsEmailadresFoutZichtbaar = true;
            return false;
        }

        IsEmailadresFoutZichtbaar = false;
        return true;
    }

    private bool HasValidTelefoonnummer()
    {
        if (string.IsNullOrWhiteSpace(Telefoonnummer))
        {
            TelefoonnummerFoutmelding = "Telefoonnummer is verplicht.";
            IsTelefoonnummerFoutZichtbaar = true;
            return false;
        }

        var digits = new string(Telefoonnummer.Where(char.IsDigit).ToArray());

        if (digits.Length < 8 || digits.Length > 15)
        {
            TelefoonnummerFoutmelding = "Telefoonnummer moet tussen 8 en 15 cijfers bevatten.";
            IsTelefoonnummerFoutZichtbaar = true;
            return false;
        }

        if (!Regex.IsMatch(Telefoonnummer, "^[0-9 +]+$"))
        {
            TelefoonnummerFoutmelding = "Alleen cijfers, spaties en '+' zijn toegestaan.";
            IsTelefoonnummerFoutZichtbaar = true;
            return false;
        }

        IsTelefoonnummerFoutZichtbaar = false;
        return true;
    }
}