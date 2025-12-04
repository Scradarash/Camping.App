using Camping.App.Views;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Camping.App.ViewModels
{
    public partial class StaanplaatsDetailViewModel : ObservableObject
    {
        private readonly IReservatieDataService _reservatieDataService;
        private readonly IAccommodatieRepository _accommodatieRepository;

        [ObservableProperty]
        private Staanplaats? staanplaats;

        [ObservableProperty]
        private string? periodeTekst;

        public ObservableCollection<Accommodatie> GeschikteAccommodaties { get; } = new();

        public StaanplaatsDetailViewModel(IReservatieDataService reservatieDataService, IAccommodatieRepository accommodatieRepository)
        {
            _reservatieDataService = reservatieDataService;
            _accommodatieRepository = accommodatieRepository;
        }

        public void Initialize(Staanplaats geselecteerdeStaanplaats)
        {
            Staanplaats = geselecteerdeStaanplaats;

            if (_reservatieDataService.StartDate.HasValue && _reservatieDataService.EndDate.HasValue)
            {
                PeriodeTekst = $"{_reservatieDataService.StartDate:dd-MM-yyyy} - {_reservatieDataService.EndDate:dd-MM-yyyy}";
            }

            FilterAccommodaties();
        }

        private void FilterAccommodaties()
        {
            GeschikteAccommodaties.Clear();
            var alleAccommodaties = _accommodatieRepository.GetAll();

            foreach (var acc in alleAccommodaties) //filter welke accomodatie op welk veld komt
            {
                bool toevoegen = false;

                if (Staanplaats.Name.Contains("Trekkersveld"))
                {
                    if (acc.Name == "Tent") toevoegen = true;
                }
                else if (Staanplaats.Name.Contains("Chaletveld"))
                {
                    if (acc.Name == "Chalet") toevoegen = true;
                }
                else
                {
                    if (acc.Name != "Chalet") toevoegen = true;
                }

                if (toevoegen)
                {
                    GeschikteAccommodaties.Add(acc);
                }
            }
        }

        [RelayCommand]
        private async Task Reserveer()
        {
            _reservatieDataService.SelectedStaanplaats = Staanplaats;

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