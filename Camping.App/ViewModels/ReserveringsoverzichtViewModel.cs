using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
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

    [ObservableProperty]
    private string periodDescription;

    [ObservableProperty]
    private string fieldName;

    // Nieuwe property om de naam van de geselecteerde staanplaats weer te geven in de View
    [ObservableProperty]
    private string staanplaatsNaam;

    [ObservableProperty]
    private Accommodatie selectedAccommodatie;

    public ObservableCollection<Accommodatie> Accommodaties { get; } = new();

    // De Service komt binnen via de constructor
    public ReserveringsoverzichtViewModel(IReservatieDataService reservatieDataService, IAccommodatieService accommodatieService, IReserveringService reserveringService)
    {
        _reservatieDataService = reservatieDataService;
        _accommodatieService = accommodatieService; // Opslaan in de variabele
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

        // HIER GEBEURT HET FILTEREN
        Accommodaties.Clear();

        // We vragen aan de service: wat mag er staan op dit veld"
        var gefilterdeLijst = _accommodatieService.GetGeschikteAccommodaties(_reservatieDataService.SelectedVeld);

        foreach (var acc in gefilterdeLijst)
        {
            // In het reserveringsoverzicht alleen accommodaties tonen die overeenkomen met de gekozen staanplaats
            if (gekozenPlek != null)
            {
                // Hier doen we de vergelijking op naam (case insensitive), beetje scuffed maar werkt voor nu
                if (acc.Name.Contains(gekozenPlek.AccommodatieType, StringComparison.OrdinalIgnoreCase)
                    || gekozenPlek.AccommodatieType.Contains(acc.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Accommodaties.Add(acc);
                }
            }
            else
            {
                // Als er geen specifieke plek is gekozen, voegen we alles toe wat op het veld mag, maar filteren we niet verder op staanplaats type
                // Dit was hoe Jos het oorspronkelijk had
                Accommodaties.Add(acc);
            }
        }

        // Selecteer automatisch de eerste optie (handig voor de gebruiker)
        if (Accommodaties.Count > 0)
        {
            SelectedAccommodatie = Accommodaties[0];
        }
    }

    [RelayCommand]
    private async Task BevestigAccommodatie()
    {
        //Validatie en daadwerkelijk opslaan van de reservering

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
            //Gekozen accommodatie tijdelijk bewaren in data service
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
}