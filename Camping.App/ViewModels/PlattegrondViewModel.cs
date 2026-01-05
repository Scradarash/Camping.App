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
    private readonly IFaciliteitService _faciliteitService;
    private readonly IReservatieDataService _reservatieDataService;
    private readonly IServiceProvider _serviceProvider;

    public ObservableCollection<Veld> Velden { get; } = new();
    public ObservableCollection<Faciliteit> Faciliteiten { get; } = new();

    public PlattegrondViewModel(
        IVeldService veldService,
        IFaciliteitService faciliteitService,
        IReservatieDataService reservatieService,
        IServiceProvider serviceProvider)
    {
        _veldService = veldService;
        _faciliteitService = faciliteitService;
        _reservatieDataService = reservatieService;
        _serviceProvider = serviceProvider;
        LoadAreas();
    }

    private void LoadAreas()
    {
        Velden.Clear();
        IEnumerable<Veld> velden = _veldService.GetAll();
        foreach (Veld veld in velden)
        {
            Velden.Add(veld);
        }

        Faciliteiten.Clear();
        IEnumerable<Faciliteit> faciliteiten = _faciliteitService.GetFaciliteiten();
        foreach (Faciliteit faciliteit in faciliteiten)
        {
            Faciliteiten.Add(faciliteit);
        }
    }

    [RelayCommand]
    private async Task SelectVeld(Veld veld)
    {
        try
        {
            //Haal de nieuwe Detail View op
            VeldDetailView detailView = _serviceProvider.GetRequiredService<VeldDetailView>();

            //Initialiseer de ViewModel met het geklikte veld
            if (detailView.BindingContext is VeldDetailViewModel vm)
            {
                vm.Initialize(veld);
            }

            //Open als Modal (Popup)
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

    [RelayCommand]
    private async Task ShowFaciliteitInfo(Faciliteit faciliteit)
    {
        // Simpele popup met info
        await Application.Current.MainPage.DisplayAlert(
            faciliteit.Name,
            faciliteit.Description,
            "Sluiten");
    }
}