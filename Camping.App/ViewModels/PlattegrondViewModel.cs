using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Camping.Core.Models;
using Camping.Core.Interfaces.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Camping.App.Views;

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
        try
        {
            // Checken of datum ingevoerd is, zoniet tonen alert met info bij klikken staanplaats
            if (!_reservatieDataService.IsValidPeriod())
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Geen datum",
                    "Selecteer eerst een datum via het kalender icoon.",
                    "OK");
                return;
            }

            // 2. Haal de nieuwe Detail View op via Dependency Injection
            var detailView = _serviceProvider.GetRequiredService<StaanplaatsDetailView>();

            // 3. Initialiseer de ViewModel met de geklikte staanplaats
            if (detailView.BindingContext is StaanplaatsDetailViewModel vm)
            {
                vm.Initialize(staanplaats);
            }

            // 4. Open als Modal (Popup)
            await Application.Current.MainPage.Navigation.PushModalAsync(detailView);
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", ex.Message, "OK");
        }
    }

    //oude display alert van Peter//

            /*// Popup voor bevestiging
            bool wantsToReserve = await Application.Current.MainPage.DisplayAlert(
                staanplaats.Name,
                $"Wilt u {staanplaats.Name} reserveren voor de periode:\n" +
                $"{_reservatieDataService.StartDate:dd-MM-yyyy} - {_reservatieDataService.EndDate:dd-MM-yyyy}?",
                "Reserveer",
                "Annuleer");

            if (wantsToReserve)
            {
                // Tijdelijk opslaan
                _reservatieDataService.SelectedStaanplaats = staanplaats;

                // Navigatie naar volgende view
                await Shell.Current.GoToAsync(nameof(ReserveringsoverzichtView));
            }
        }
        catch (Exception ex)
        {
            // Tijdelijk: toon de echte fout
            await Application.Current.MainPage.DisplayAlert(
                "Er ging iets mis",
                ex.ToString(),   // of ex.Message voor korter
                "OK");
        }*/
   // }

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