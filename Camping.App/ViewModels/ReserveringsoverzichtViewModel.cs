using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Camping.Core.Interfaces.Repositories;
using System.Collections.ObjectModel;

namespace Camping.App.ViewModels;

public partial class ReserveringsoverzichtViewModel : ObservableObject
{
    private readonly IReservatieDataService _reservatieDataService;
    private readonly IAccommodatieRepository _accommodatieRepository;

    [ObservableProperty]
    private string periodDescription;

    [ObservableProperty]
    private string fieldName;

    [ObservableProperty]
    private Accommodatie selectedAccommodatie;

    public ObservableCollection<Accommodatie> Accommodaties { get; } = new();

    public ReserveringsoverzichtViewModel(IReservatieDataService reservatieDataService, IAccommodatieRepository accommodatieRepository)
    {
        _reservatieDataService = reservatieDataService;
        _accommodatieRepository = accommodatieRepository;

        LoadData();
    }

    private void LoadData()
    {
        if (_reservatieDataService.SelectedStaanplaats != null && _reservatieDataService.StartDate != null)
        {
            FieldName = _reservatieDataService.SelectedStaanplaats.Name;
            PeriodDescription = $"{_reservatieDataService.StartDate:dd-MM-yyyy} tot {_reservatieDataService.EndDate:dd-MM-yyyy}";
        }

        // checkboxen
        Accommodaties.Clear();
        foreach (var acc in _accommodatieRepository.GetAll())
        {
            Accommodaties.Add(acc);
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