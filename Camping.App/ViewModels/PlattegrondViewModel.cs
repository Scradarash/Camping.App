using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Camping.Core.Models;
using Camping.Core.Interfaces.Services;
using Camping.App.Views;

namespace Camping.App.ViewModels;

public partial class PlattegrondViewModel : ObservableObject
{
    private readonly IVeldService _veldService;
    private readonly IReservatieDataService _reservatieDataService;
    private readonly IServiceProvider _serviceProvider;
    public ObservableCollection<Veld> Velden { get; } = new();

    public PlattegrondViewModel(IVeldService veldService, IReservatieDataService reservatieService, IServiceProvider serviceProvider)
    {
        _veldService = veldService;
        _reservatieDataService = reservatieService;
        _serviceProvider = serviceProvider;
        LoadAreas();
    }

    private void LoadAreas()
    {
        Velden.Clear();
        // Aanname dat GetAll een IEnumerable of List teruggeeft
        IEnumerable<Veld> velden = _veldService.GetAll();
        foreach (Veld v in velden)
        {
            Velden.Add(v);
        }
    }

    [RelayCommand]
    private async Task SelectVeld(Veld veld)
    {
        try
        {
            // 2. Haal de nieuwe Detail View op via Dependency Injection
            VeldDetailView detailView = _serviceProvider.GetRequiredService<VeldDetailView>();

            // 3. Initialiseer de ViewModel met het geklikte veld
            if (detailView.BindingContext is VeldDetailViewModel vm)
            {
                vm.Initialize(veld);
            }

            // 4. Open als Modal (Popup)
            await Application.Current.MainPage.Navigation.PushModalAsync(detailView);
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Fout", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task OpenKalender()
    {
        KalenderView kalenderView = _serviceProvider.GetRequiredService <Views.KalenderView>();


        await Application.Current.MainPage.Navigation.PushModalAsync(kalenderView);
    }
}