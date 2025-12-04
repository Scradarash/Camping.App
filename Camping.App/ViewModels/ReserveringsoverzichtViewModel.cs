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
        // Veiligheidscheck: als er geen staanplaats is, stop dan (voorkomt crashes)
        if (_reservatieDataService.SelectedStaanplaats == null || _reservatieDataService.StartDate == null)
        {
            return;
        }

        FieldName = _reservatieDataService.SelectedStaanplaats.Name;
        PeriodDescription = $"{_reservatieDataService.StartDate:dd-MM-yyyy} tot {_reservatieDataService.EndDate:dd-MM-yyyy}";

        // HIER GEBEURT HET FILTEREN
        Accommodaties.Clear();

        // We vragen aan de service: wat mag er staan op dit veld"
        var gefilterdeLijst = _accommodatieService.GetGeschikteAccommodaties(_reservatieDataService.SelectedStaanplaats);

        foreach (var acc in gefilterdeLijst)
        {
            Accommodaties.Add(acc);
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

        await Application.Current.MainPage.DisplayAlert("Volgende stap", $"U heeft gekozen voor: {SelectedAccommodatie.Name}. \nNu naar voorzieningen...", "OK");
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}