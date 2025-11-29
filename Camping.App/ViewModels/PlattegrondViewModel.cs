using System.Collections.ObjectModel;
using Camping.Core.Models;
using Camping.Core.Interfaces.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Camping.App.ViewModels;

public partial class PlattegrondViewModel : ObservableObject
{
    private readonly IStaanplaatsService _service;

    public ObservableCollection<Staanplaats> Staanplaatsen { get; } = new();

    public PlattegrondViewModel(IStaanplaatsService service)
    {
        _service = service;
        LoadAreas();
    }

    private void LoadAreas()
    {
        Staanplaatsen.Clear();
        var plants = _service.GetAll();
        foreach (var p in plants)
        {
            Staanplaatsen.Add(p);
        }
    }

    [RelayCommand]
    private async Task SelectStaanplaats(Staanplaats plaats)
    {
        await Application.Current.MainPage.DisplayAlert(
            plaats.Name,
            $"Het {plaats.Name.ToLower()} is een prachtige staanplaats voor al uw campeergenot",
            "OK");
    }

    [RelayCommand]
    private async Task OpenKalender()
    {
        await Application.Current.MainPage.DisplayAlert("Kalender", "Hier opent de kalender logic", "OK");
    }

    [RelayCommand]
    private void ExitApp()
    {
        System.Environment.Exit(0);
    }
}