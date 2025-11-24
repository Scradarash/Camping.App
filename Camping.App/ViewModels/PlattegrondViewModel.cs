using System.Collections.ObjectModel;
using Camping.Core.Models;
using Camping.Core.Interfaces.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Camping.App.ViewModels;

public partial class PlattegrondViewModel : ObservableObject
{
    public ObservableCollection<ClickArea> Areas { get; } = new();

    private readonly IStaanplaatsRepository _repository;

    public PlattegrondViewModel(IStaanplaatsRepository repository)
    {
        _repository = repository;
        LoadAreas();
    }

    private void LoadAreas()
    {
        var staanplaatsen = _repository.GetAllStaanplaatsen();

        foreach (var plaats in staanplaatsen)
        {
            Areas.Add(plaats);
        }
    }
}
