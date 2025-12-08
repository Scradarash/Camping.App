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
    public ReserveringsoverzichtViewModel(IReservatieDataService reservatieDataService, IAccommodatieService accommodatieService)
    {
        _reservatieDataService = reservatieDataService;
        _accommodatieService = accommodatieService; // Opslaan in de variabele

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
    private async Task ConfirmAccommodation()
    {
        if (SelectedAccommodatie == null)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", "Kies een accommodatie type.", "OK");
            return;
        }

        // Pop-up tonen veranderd om ook de plek info te tonen als die er is
        string plekInfo = _reservatieDataService.SelectedStaanplaats != null
            ? $" op plek {_reservatieDataService.SelectedStaanplaats.id}"
            : "";

        await Application.Current.MainPage.DisplayAlert("Volgende stap", $"U heeft gekozen voor: {SelectedAccommodatie.Name}{plekInfo}. \nNu naar voorzieningen...", "OK");
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}