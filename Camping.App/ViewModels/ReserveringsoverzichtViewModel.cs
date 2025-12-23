using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
using Camping.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Camping.App.ViewModels;

public partial class ReserveringsoverzichtViewModel : ObservableObject
{
    //Voor het onthouden van alle eerdere keuzes (reserveringsperiode, veld, staanplaats)
    private readonly IReservatieDataService _reservatieDataService;

    private readonly IAccommodatieService _accommodatieService;
    private readonly IReserveringService _reserveringService;

    //Services voor validatie gegevens en prijsberekening
    private readonly ReserveringshouderValidatieService _validatieService;
    private readonly PrijsBerekenService _prijsBerekenService;

    //Voor header met periode tekst en veldnaam
    [ObservableProperty]
    private string periodDescription;

    [ObservableProperty]
    private string fieldName;

    //Voor geselecteerde staanplaats info tonen
    [ObservableProperty]
    private string staanplaatsInfo;

    //Geselecteerde accommodatie bijhouden
    [ObservableProperty]
    private Accommodatie? selectedAccommodatie;

    //Invoer reserveringshouder naam
    [ObservableProperty]
    private string naam;

    [ObservableProperty]
    private string naamFoutmelding;

    [ObservableProperty]
    private bool isNaamFoutZichtbaar;

    //Invoer reserveringshouder geboortedatum
    [ObservableProperty]
    private DateTime? geboortedatum;

    [ObservableProperty]
    private string geboortedatumFoutmelding;

    [ObservableProperty]
    private bool isGeboortedatumFoutZichtbaar;

    //Invoer reserveringshouder email
    [ObservableProperty]
    private string emailadres;

    [ObservableProperty]
    private string emailadresFoutmelding;

    [ObservableProperty]
    private bool isEmailadresFoutZichtbaar;

    //Invoer reserveringshouder telefoon
    [ObservableProperty]
    private string telefoonnummer;

    [ObservableProperty]
    private string telefoonnummerFoutmelding;

    [ObservableProperty]
    private bool isTelefoonnummerFoutZichtbaar;

    //Voor later bepalen of voorzieningen checkboxes beschikbaar zijn voor de staanplaats of verborgen moeten worden
    [ObservableProperty]
    private bool isStroomMogelijk;

    [ObservableProperty]
    private bool isWaterMogelijk;

    //Geselecteerde voorzieningen
    [ObservableProperty]
    private bool kiestStroom;

    [ObservableProperty]
    private bool kiestWater;

    //Totaalprijs wordt herberekend bij keuzes doordat NotifyPropertyChangedFor de prijs tekst ook verandert
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotaalPrijsTekst))]
    private decimal totaalPrijs;

    //Teksten die in de UI getoond worden bij de voorzieningen en totaalprijs
    public decimal StroomPrijsPerNacht => _prijsBerekenService.StroomPrijsPerNacht;
    public decimal WaterPrijsPerNacht => _prijsBerekenService.WaterPrijsPerNacht;

    public string TotaalPrijsTekst => $"Totaalprijs: €{TotaalPrijs:F2}";

    //Lijst met mogelijke accommodaties
    public ObservableCollection<Accommodatie> Accommodaties { get; } = new();

    //Lijst van de prijsregels (staanplaats, accommodatie,a voorzieningen)
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
        LoadContextHeader();
        LoadStaanplaatsInfo();
        LoadAccommodaties();
        LoadReserveringshouderDefaults();

        RecalculatePrijs();
    }

    private void LoadContextHeader()
    {
        //Header met veldnaam en reserveringsperiode tekst
        FieldName = _reservatieDataService.SelectedVeld!.Name;

        DateTime start = _reservatieDataService.StartDate!.Value;
        DateTime end = _reservatieDataService.EndDate!.Value;

        PeriodDescription = $"{start:dd-MM-yyyy} tot {end:dd-MM-yyyy}";
    }

    private void LoadStaanplaatsInfo()
    {
        SetStaanplaatsInfoText();
        SetVoorzieningBeschikbaarheid();
        RestoreVoorzieningKeuzes();
    }

    private void SetStaanplaatsInfoText()
    {
        var gekozenStaanplaats = _reservatieDataService.SelectedStaanplaats!;
        StaanplaatsInfo = $"Plaats: {gekozenStaanplaats.id} ({gekozenStaanplaats.AccommodatieType})";
    }

    private void SetVoorzieningBeschikbaarheid()
    {
        var gekozenStaanplaats = _reservatieDataService.SelectedStaanplaats!;
        IsStroomMogelijk = gekozenStaanplaats.HeeftStroom;
        IsWaterMogelijk = gekozenStaanplaats.HeeftWater;
    }

    private void RestoreVoorzieningKeuzes()
    {
        // Alleen terugzetten als het ook echt mogelijk is op deze plek
        KiestStroom = _reservatieDataService.KiestStroom && IsStroomMogelijk;
        KiestWater = _reservatieDataService.KiestWater && IsWaterMogelijk;
    }


    private void LoadAccommodaties()
    {
        var staanplaatsId = GetSelectedStaanplaatsId();
        var geschikteAccommodaties = GetGeschikteAccommodaties(staanplaatsId);

        FillAccommodaties(geschikteAccommodaties);
        RestoreSelectedAccommodatie();
    }

    private int GetSelectedStaanplaatsId()
    {
        return _reservatieDataService.SelectedStaanplaats!.id;
    }

    private IEnumerable<Accommodatie> GetGeschikteAccommodaties(int staanplaatsId)
    {
        return _accommodatieService.GetGeschikteAccommodatiesVoorStaanplaats(staanplaatsId);
    }

    private void FillAccommodaties(IEnumerable<Accommodatie> accommodaties)
    {
        Accommodaties.Clear();
        foreach (var acc in accommodaties)
            Accommodaties.Add(acc);
    }

    private void RestoreSelectedAccommodatie()
    {
        //Bewaar eerdere keuze door gebruiker (bij terug navigeren)
        var eerdereKeuze = _reservatieDataService.SelectedAccommodatie;

        SelectedAccommodatie = eerdereKeuze != null
            ? Accommodaties.FirstOrDefault(a => a.Id == eerdereKeuze.Id)
            : null;
    }


    private void LoadReserveringshouderDefaults()
    {
        //Gegevens bewaren in de velden (als gebruiker terug/vooruit navigeert tijdens reserveren of foutmelding ziet)
        Naam = _reservatieDataService.Naam;
        Geboortedatum = _reservatieDataService.Geboortedatum;
        Emailadres = _reservatieDataService.Emailadres;
        Telefoonnummer = _reservatieDataService.Telefoonnummer;
    }

    private void RecalculatePrijs() //TODO Splitsen omdat dit nu 4 dingen doet, context ophalen, input voor berekening samenstellen, berekening uitvoeren en UI updaten
    {
        DateTime start = _reservatieDataService.StartDate!.Value;
        DateTime end = _reservatieDataService.EndDate!.Value;

        var staanplaats = _reservatieDataService.SelectedStaanplaats!;

        decimal staanplaatsPrijs = staanplaats.Prijs;

        //Accommodatie prijs (automatisch op 0 want accommodatie is nog niet geselecteerd)
        decimal accommodatiePrijs = SelectedAccommodatie?.Prijs ?? 0m;

        //Bereken totaal + regels prijs
        var (totaal, regels) = _prijsBerekenService.Bereken(
            start,
            end,
            staanplaatsPrijs,
            accommodatiePrijs,
            KiestStroom,
            IsStroomMogelijk,
            KiestWater,
            IsWaterMogelijk);

        //Prijs lijst per regel bijwerken
        PrijsInfo.Clear();
        foreach (var regel in regels)
            PrijsInfo.Add(regel);

        //Totaalprijs bijwerken
        TotaalPrijs = totaal;
    }

    //Als gebruiker een andere accommodatie kiest, wordt de prijs opnieuw berekenen
    partial void OnSelectedAccommodatieChanged(Accommodatie? value) => RecalculatePrijs();

    //Als gebruiker stroom/water selecteert/deselecteert, wordt de prijs opnieuw berekenen
    partial void OnKiestStroomChanged(bool value) => RecalculatePrijs();
    partial void OnKiestWaterChanged(bool value) => RecalculatePrijs();

    [RelayCommand]
    private async Task VoltooiReservering()
    {
        if (!ValidateReserveringshouderInput())
            return;

        if (SelectedAccommodatie is null)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Accommodatie vereist",
                "Kies eerst een accommodatie type voordat je verder gaat.",
                "OK");
            return;
        }

        try
        {
            PrepareReservering();

            await SaveReserveringAndFinishAsync();
        }
        catch (Exception ex)
        {
            await ShowSaveErrorAsync(ex);
        }
    }

    private bool ValidateReserveringshouderInput()
    {
        bool naamOk = HasValidNaam();
        bool geboortedatumOk = HasValidGeboortedatum();
        bool emailOk = HasValidEmailadres();
        bool telefoonOk = HasValidTelefoonnummer();

        return naamOk && geboortedatumOk && emailOk && telefoonOk;
    }

    private void PrepareReservering()
    {
        SaveWizardData();

        RecalculatePrijs();
    }

    private async Task SaveReserveringAndFinishAsync() //TODO Kan nog gesplitst worden in reservering maken en navigatie + foutmelding methodes apart (hoeft niet want is al best clean)
    {
        //Reservering maken
        await _reserveringService.MaakReserveringAsync(
            _reservatieDataService.StartDate!.Value,
            _reservatieDataService.EndDate!.Value,
            _reservatieDataService.SelectedVeld!,
            _reservatieDataService.SelectedStaanplaats!,
            SelectedAccommodatie!,
            KiestStroom,
            KiestWater,
            TotaalPrijs);

        //Feedback naar gebruiker
        await SuccessMessageReservatie();

        //Navigatie terug naar plattegrond
        await Shell.Current.GoToAsync("//PlattegrondView");
    }

    private static async Task SuccessMessageReservatie()
    {
        await Application.Current.MainPage.DisplayAlert(
            "Reservering voltooid",
            "De reservering is succesvol opgeslagen.",
            "OK");
    }


    private Task ShowSaveErrorAsync(Exception ex)
    {
        return Application.Current.MainPage.DisplayAlert(
            "Fout bij opslaan",
            $"Er ging iets mis bij het opslaan van de reservering:\n{ex.Message}",
            "OK");
    }


    private void SaveWizardData()
    {
        //Eerdere keuzes tijdenlijk bewaren in de ReservatieDataService
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

        EmailadresFoutmelding = string.Empty;
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

        TelefoonnummerFoutmelding = string.Empty;
        IsTelefoonnummerFoutZichtbaar = false;
        return true;
    }
}