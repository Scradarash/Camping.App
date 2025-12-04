using Camping.App.Views;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Camping.App.ViewModels
{
    public partial class VeldDetailViewModel : ObservableObject
    {
        private readonly IReservatieDataService _reservatieDataService;
        private readonly IAccommodatieService _accommodatieService; // Gebruik Service ipv Repository

        [ObservableProperty]
        private Veld? veld;

        [ObservableProperty]
        private string? periodeTekst;

        public ObservableCollection<Accommodatie> GeschikteAccommodaties { get; } = new();

        // Constructor injecteert nu de Service
        public VeldDetailViewModel(IReservatieDataService reservatieDataService, IAccommodatieService accommodatieService)
        {
            _reservatieDataService = reservatieDataService;
            _accommodatieService = accommodatieService;
        }

        public void Initialize(Veld geselecteerdeVeld)
        {
            Veld = geselecteerdeVeld;

            if (_reservatieDataService.StartDate.HasValue && _reservatieDataService.EndDate.HasValue)
            {
                PeriodeTekst = $"{_reservatieDataService.StartDate:dd-MM-yyyy} - {_reservatieDataService.EndDate:dd-MM-yyyy}";
            }

            GeschikteAccommodaties.Clear();
            var lijst = _accommodatieService.GetGeschikteAccommodaties((Veld)Veld);

            foreach (var item in lijst)
            {
                GeschikteAccommodaties.Add(item);
            }
        }

        [RelayCommand]
        private async Task Reserveer()
        {
            _reservatieDataService.SelectedVeld = Veld;

            await Application.Current.MainPage.Navigation.PopModalAsync();
            await Shell.Current.GoToAsync(nameof(ReserveringsoverzichtView));
        }

        [RelayCommand]
        private async Task Annuleer()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}