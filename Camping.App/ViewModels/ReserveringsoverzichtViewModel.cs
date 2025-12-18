using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
using Camping.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Camping.App.ViewModels;

public partial class ReserveringsoverzichtViewModel : ObservableObject
{
    private readonly IReservatieDataService _reservatieDataService;

    // gebruiken de Service voor de logica (ipv de Repository)
    private readonly IAccommodatieService _accommodatieService;
    private readonly IReserveringService _reserveringService;

    private readonly ReserveringshouderValidatieService _validatieService;
    private readonly PrijsBerekenService _prijsBerekenService;

    [ObservableProperty]
    private string periodDescription;

    [ObservableProperty]
    private string fieldName;

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

    [ObservableProperty]
    private bool kiestStroom;

    [ObservableProperty]
    private bool kiestWater;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotaalPrijsTekst))]
    private decimal totaalPrijs;

    public string StroomPrijsTekst => $"€{_prijsBerekenService.StroomPrijsPerNacht:F2},- per nacht";
    public string WaterPrijsTekst => $"€{_prijsBerekenService.WaterPrijsPerNacht:F2},- per nacht";
    public string TotaalPrijsTekst => $"Totaalprijs: €{TotaalPrijs:F2}";

    public ObservableCollection<Accommodatie> Accommodaties { get; } = new();
    public ObservableCollection<PrijsInfo> PrijsInfo { get; } = new();

    public ReserveringsoverzichtViewModel(
        IReservatieDataService reservatieDataService,
        IAccommodatieService accommodatieService,
        IReserveringService reserveringService,
        ReserveringshouderValidatieService validatieService,
        PrijsBerekenService prijsBerekenService)
    {
        _reservatieDataService = reservatieDataService;
        _accommodatieService = accommodatieService;
        _reserveringService = reserveringService;
        _validatieService = validatieService;
        _prijsBerekenService = prijsBerekenService;

        LoadData();
    }

    private void LoadData()
    {
        if (!HasMinimumContext())
            return;

        LoadContextHeader();
        LoadStaanplaatsInfo();
        LoadAccommodaties();
        LoadReserveringshouderDefaults();

        RecalculatePrijs();
    }

    private bool HasMinimumContext()
    {
        // Zonder deze info kan de pagina niet netjes vullen.
        if (_reservatieDataService.SelectedVeld == null)
            return false;

        if (_reservatieDataService.StartDate == null || _reservatieDataService.EndDate == null)
            return false;

        return true;
    }

    private void LoadContextHeader()
    {
        FieldName = _reservatieDataService.SelectedVeld!.Name;
        PeriodDescription = $"{_reservatieDataService.StartDate:dd-MM-yyyy} tot {_reservatieDataService.EndDate:dd-MM-yyyy}";
    }

    private void LoadStaanplaatsInfo()
    {
        var gekozenPlek = _reservatieDataService.SelectedStaanplaats;

        if (gekozenPlek != null)
        {
            StaanplaatsNaam = $"Plaats: {gekozenPlek.id} ({gekozenPlek.AccommodatieType})";

            // Checken wat de plek daadwerkelijk heeft (uit DB data / of later uit uitbreiding)
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
    }

    private void LoadAccommodaties()
    {
        Accommodaties.Clear();

        var veld = _reservatieDataService.SelectedVeld!;
        var gekozenPlek = _reservatieDataService.SelectedStaanplaats;

        // DB-gedreven geschikte accommodaties voor dit veld.
        var geschikte = _accommodatieService.GetGeschikteAccommodaties(veld);

        foreach (var acc in FilterOpStaanplaatsTypes(geschikte, gekozenPlek))
        {
            Accommodaties.Add(acc);
        }

        if (Accommodaties.Count > 0)
        {
            SelectedAccommodatie = Accommodaties[0];
        }
    }

    private IEnumerable<Accommodatie> FilterOpStaanplaatsTypes(IEnumerable<Accommodatie> accommodaties, Staanplaats? plek)
    {
        // Als er geen plek is gekozen, dan is veld-filtering genoeg.
        if (plek == null || string.IsNullOrWhiteSpace(plek.AccommodatieType))
            return accommodaties;

        // plek.AccommodatieType is iets als: "Tent, Caravan"
        // Filter: alleen accommodaties die in die string voorkomen.
        return accommodaties.Where(a =>
            plek.AccommodatieType.Contains(a.Name, StringComparison.OrdinalIgnoreCase));
    }

    private void LoadReserveringshouderDefaults()
    {
        Naam = _reservatieDataService.Naam;
        Geboortedatum = _reservatieDataService.Geboortedatum;
        Emailadres = _reservatieDataService.Emailadres;
        Telefoonnummer = _reservatieDataService.Telefoonnummer;
    }

    private void RecalculatePrijs()
    {
        PrijsInfo.Clear();

        if (_reservatieDataService.StartDate == null || _reservatieDataService.EndDate == null)
        {
            TotaalPrijs = 0m;
            return;
        }

        var plek = _reservatieDataService.SelectedStaanplaats;

        // Als er nog geen plek is, kunnen we nog geen echte staanplaatsprijs tonen.
        decimal staanplaatsPrijs = plek?.Prijs ?? 0m;

        decimal accommodatiePrijs = SelectedAccommodatie?.Prijs ?? 0m;

        var (totaal, regels) = _prijsBerekenService.Bereken(
            _reservatieDataService.StartDate.Value,
            _reservatieDataService.EndDate.Value,
            staanplaatsPrijs,
            accommodatiePrijs,
            KiestStroom,
            IsStroomMogelijk,
            KiestWater,
            IsWaterMogelijk);

        foreach (var regel in regels)
            PrijsInfo.Add(regel);

        TotaalPrijs = totaal;
    }

    partial void OnSelectedAccommodatieChanged(Accommodatie value)
    {
        RecalculatePrijs();
    }

    partial void OnKiestStroomChanged(bool value)
    {
        RecalculatePrijs();
    }

    partial void OnKiestWaterChanged(bool value)
    {
        RecalculatePrijs();
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
            PersistWizardData();

            // Zorg dat de prijs actueel is vlak voor opslaan
            RecalculatePrijs();

            await _reserveringService.MaakReserveringAsync(
                _reservatieDataService.StartDate.Value,
                _reservatieDataService.EndDate.Value,
                _reservatieDataService.SelectedVeld,
                _reservatieDataService.SelectedStaanplaats,
                SelectedAccommodatie,
                KiestStroom,
                KiestWater,
                TotaalPrijs);

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

    private void PersistWizardData()
    {
        _reservatieDataService.Naam = Naam;
        _reservatieDataService.Geboortedatum = Geboortedatum;
        _reservatieDataService.Emailadres = Emailadres;
        _reservatieDataService.Telefoonnummer = Telefoonnummer;
        _reservatieDataService.SelectedAccommodatie = SelectedAccommodatie;
        _reservatieDataService.KiestStroom = KiestStroom;
        _reservatieDataService.KiestWater = KiestWater;
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
