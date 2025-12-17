using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
using Camping.Core.Services; 
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

    private readonly ReserveringshouderValidatieService _validatieService;
    // Prijzen definities (hardcoded voor nu, kan later uit DB/Service)
    private const decimal PRIJS_BASIS_PER_NACHT = 20.00m;
    private const decimal PRIJS_STROOM = 4.99m;
    private const decimal PRIJS_WATER = 0.00m;

    [ObservableProperty]
    private string periodDescription;

    [ObservableProperty]
    private string fieldName;

    [ObservableProperty]
    private string staanplaatsNaam;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotaalPrijsTekst))]
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

    public string StroomPrijsTekst => $"€{PRIJS_STROOM},- per nacht";
    public string WaterPrijsTekst => $"€{PRIJS_WATER},- per nacht";

    public string TotaalPrijsTekst => $"Totaalprijs: €{BerekenEindTotaal():F2}";

    public ObservableCollection<Accommodatie> Accommodaties { get; } = new();

    public ReserveringsoverzichtViewModel(
        IReservatieDataService reservatieDataService,
        IAccommodatieService accommodatieService,
        IReserveringService reserveringService,
        ReserveringshouderValidatieService validatieService)
    {
        _reservatieDataService = reservatieDataService;
        _accommodatieService = accommodatieService;
        _reserveringService = reserveringService;

        _validatieService = validatieService;

        LoadData();
    }

    private void LoadData()
    {
        if (_reservatieDataService.SelectedVeld == null || _reservatieDataService.StartDate == null)
        {
            return;
        }

        FieldName = _reservatieDataService.SelectedVeld.Name;
        PeriodDescription = $"{_reservatieDataService.StartDate:dd-MM-yyyy} tot {_reservatieDataService.EndDate:dd-MM-yyyy}";

        var gekozenPlek = _reservatieDataService.SelectedStaanplaats;

        if (gekozenPlek != null)
        {
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

        var gefilterdeLijst = _accommodatieService.GetGeschikteAccommodaties(_reservatieDataService.SelectedVeld);

        foreach (var acc in gefilterdeLijst)
        {
            // tijdelijke hardcoded prijzen van de accommodaties
            if (acc.Name.Equals("Tent", StringComparison.OrdinalIgnoreCase))
                acc.Prijs = 2.99m;
            else if (acc.Name.Equals("Caravan", StringComparison.OrdinalIgnoreCase))
                acc.Prijs = 6.99m;
            else if (acc.Name.Equals("Camper", StringComparison.OrdinalIgnoreCase))
                acc.Prijs = 7.99m;
            else if (acc.Name.Equals("Chalet", StringComparison.OrdinalIgnoreCase))
                acc.Prijs = 49.99m;
            else
                acc.Prijs = 0.00m;

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

        // Accommodatieprijs
        if (SelectedAccommodatie != null)
        {
            totaal += (nachten * SelectedAccommodatie.Prijs);
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
        var result = _validatieService.ValidateNaam(Naam);

        if (!result.IsValid)
        {
            NaamFoutmelding = result.Error;
            IsNaamFoutZichtbaar = true;
            return false;
        }

        NaamFoutmelding = string.Empty;
        IsNaamFoutZichtbaar = false;
        return true;
    }

    private bool HasValidGeboortedatum()
    {
        var result = _validatieService.ValidateGeboortedatum(Geboortedatum);

        if (!result.IsValid)
        {
            GeboortedatumFoutmelding = result.Error;
            IsGeboortedatumFoutZichtbaar = true;
            return false;
        }

        GeboortedatumFoutmelding = string.Empty;
        IsGeboortedatumFoutZichtbaar = false;
        return true;
    }

    private bool HasValidEmailadres()
    {
        var result = _validatieService.ValidateEmailadres(Emailadres);

        if (!result.IsValid)
        {
            EmailadresFoutmelding = result.Error;
            IsEmailadresFoutZichtbaar = true;
            return false;
        }

        IsEmailadresFoutZichtbaar = false;
        return true;
    }

    private bool HasValidTelefoonnummer()
    {
        var result = _validatieService.ValidateTelefoonnummer(Telefoonnummer);

        if (!result.IsValid)
        {
            TelefoonnummerFoutmelding = result.Error;
            IsTelefoonnummerFoutZichtbaar = true;
            return false;
        }

        IsTelefoonnummerFoutZichtbaar = false;
        return true;
    }
}