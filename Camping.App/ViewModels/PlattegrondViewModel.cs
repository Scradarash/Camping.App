using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Camping.Core.Models;
using Camping.Core.Interfaces.Repositories;

namespace Camping.App.ViewModels
{
    public partial class PlattegrondViewModel : ObservableObject
    {
        public ObservableCollection<Staanplaats> Staanplaatsen { get; } = new();

        private readonly CampingPlattegrond _plattegrond;

        public event Action<Staanplaats>? StaanplaatsGeselecteerd;

        public PlattegrondViewModel(IStaanplaatsRepository repo)
        {
            _plattegrond = new CampingPlattegrond(repo);
            Load();
        }

        private void Load()
        {
            foreach (var s in _plattegrond.Staanplaatsen)
                Staanplaatsen.Add(s);
        }

        [RelayCommand]
        private void Select(Staanplaats plaats)
        {
            StaanplaatsGeselecteerd?.Invoke(plaats);
        }
    }
}
