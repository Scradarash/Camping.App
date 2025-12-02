using System.Collections.ObjectModel;
using Camping.Core.Models;
using Camping.Core.Interfaces.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Camping.App.ViewModels;

public partial class PlattegrondViewModel : ObservableObject
{
    private readonly IStaanplaatsService _staanplaatsService;
    private readonly IReservatieDataService _reservatieDataService;
    private readonly IServiceProvider _serviceProvider;
    public ObservableCollection<Staanplaats> Staanplaatsen { get; } = new();

    public PlattegrondViewModel(IStaanplaatsService staanplaatsService, IReservatieDataService reservatieService, IServiceProvider serviceProvider)
    {
        _staanplaatsService = staanplaatsService;
        _reservatieDataService = reservatieService;
        _serviceProvider = serviceProvider;
        LoadAreas();
    }

    private void LoadAreas()
    {
        Staanplaatsen.Clear();
        var staanplaatsen = _staanplaatsService.GetAll();
        foreach (var p in staanplaatsen)
        {
            Staanplaatsen.Add(p);
        }
    }

    [RelayCommand]
    private async Task SelectStaanplaats(Staanplaats staanplaats)
    {
        // CHecken of datum ingevoerd is, zoniet tonen alert met info bij klikken staanplaats
        if (!_reservatieDataService.IsValidPeriod())
        {
            await Application.Current.MainPage.DisplayAlert("Geen datum", "Selecteer eerst een datum via het kalender icoon.", "OK");
            return;
        }

        // Popup toont met reserveren bij klikken staanplaats. Popup toont reserveringsperiode, mogelijheid om te reserveren of annulleren
        // (Heel lang en kan waarschijnlijk anders)
        bool wantsToReserve = await Application.Current.MainPage.DisplayAlert(staanplaats.Name,$"Wilt u {staanplaats.Name} reserveren voor de periode:\n" +
        $"{_reservatieDataService.StartDate:dd-MM-yyyy} - {_reservatieDataService.EndDate:dd-MM-yyyy}?", "Reserveer", "Annuleer");

        if (wantsToReserve)
        {
            //Als op reservering wordt geklikt,  wordt de geselecteerde data staanplaats tijdelijk opgeslagen voor de reserveringsproces
            _reservatieDataService.SelectedStaanplaats = staanplaats;

            
            // Navigatie naar volgende view
            await Shell.Current.GoToAsync("ReserveringsoverzichtView");
        }

    }

    [RelayCommand]
    private async Task OpenKalender()
    {
        var kalenderView = _serviceProvider.GetRequiredService <Views.KalenderView>();


        await Application.Current.MainPage.Navigation.PushModalAsync(kalenderView);
    }



    [RelayCommand]
    private void ExitApp()
    {
        System.Environment.Exit(0);
    }
}