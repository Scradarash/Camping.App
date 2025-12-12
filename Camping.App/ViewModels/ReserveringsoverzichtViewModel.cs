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
        }
        else
        {
            StaanplaatsNaam = "Geen specifieke plaats gekozen";
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
    }

    [RelayCommand]
    private async Task BevestigAccommodatie()
    {
        //Validatie en daadwerkelijk opslaan van de reservering
        if (!HasValidNaam() || !HasValidGeboortedatum() || !HasValidEmailadres())
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
            //Gekozen gegevens tijdelijk bewaren in data service
            _reservatieDataService.Naam = Naam;
            _reservatieDataService.SelectedAccommodatie = SelectedAccommodatie;

            //Reservering opslaan via de ReservatieService
            _reserveringService.MaakReservering(
                _reservatieDataService.StartDate.Value,
                _reservatieDataService.EndDate.Value,
                _reservatieDataService.SelectedVeld,
                _reservatieDataService.SelectedStaanplaats,
                SelectedAccommodatie);

            await Application.Current.MainPage.DisplayAlert(
                "Reservering voltooid",
                "De reservering is succesvol opgeslagen.",
                "OK");

            //Na opslaan terug naar de PlattegrondView
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

}