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
        private readonly IStaanplaatsRepository _staanplaatsRepository;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private Veld? veld;

        [ObservableProperty]
        private string? periodeTekst;

        // ObservableProperty voor het tonen van gekozen staanplaats.
        [ObservableProperty]
        private string geselecteerdeStaanplaatsTekst = "Kies een plaats op de kaart";

        public ObservableCollection<Staanplaats> Staanplaatsen { get; } = new();

        [ObservableProperty]
        private Staanplaats? geselecteerdeStaanplaats;

        public VeldDetailViewModel(
            IReservatieDataService reservatieDataService,
            IStaanplaatsRepository staanplaatsRepository,
            IServiceProvider serviceProvider)
        {
            _reservatieDataService = reservatieDataService;
            _staanplaatsRepository = staanplaatsRepository;
            _serviceProvider = serviceProvider;
        }

        public void Initialize(Veld geselecteerdeVeld)
        {
            // Veldinfo moet inzichtelijk zijn, ook als er geen datum is geselecteerd
            Veld = geselecteerdeVeld;
            PeriodeTekst = "Nog geen datum geselecteerd";

            // GeselecteerdeStaanplaats leegmaken
            GeselecteerdeStaanplaats = null;
            GeselecteerdeStaanplaatsTekst = "Kies een staanplaats op de kaart";

            // Als er al een periode is geselecteerd, dan wordt die hier getoond bovenaan
            if (_reservatieDataService.StartDate.HasValue && _reservatieDataService.EndDate.HasValue)
            {
                PeriodeTekst = $"{_reservatieDataService.StartDate:dd-MM-yyyy} - {_reservatieDataService.EndDate:dd-MM-yyyy}";
            }

            Staanplaatsen.Clear();
            // Haal alle staanplaatsen op voor dit veld
            var plekken = _staanplaatsRepository.GetByVeldId(
                Veld.id,
                _reservatieDataService.StartDate,
                _reservatieDataService.EndDate
            );

            foreach (var plek in plekken)
            {
                Staanplaatsen.Add(plek);
            }
        }

        [RelayCommand]
        //Klikken op een staanplaats
        private async Task KiesStaanplaats(Staanplaats plek)
        {
            GeselecteerdeStaanplaats = plek;
            _reservatieDataService.SelectedStaanplaats = plek;
            GeselecteerdeStaanplaatsTekst = $"Geselecteerde plaats: {plek.id}";
        }

        [RelayCommand]
        private async Task Reserveer()
        {
            // Pas bij reserveren de checks uitvoeren, zodat de gebruiker eerst de veldinfo kan bekijken
            if (!_reservatieDataService.StartDate.HasValue || !_reservatieDataService.EndDate.HasValue)
            {
                // Als er geen periode is geselecteerd, toon een melding en geef optie om kalender te openen
                bool openKalender = await Application.Current.MainPage.DisplayAlert(
                    "Geen datum",
                    "U moet een periode selecteren om te kunnen reserveren.",
                    "Annuleer", "Open Kalender");

                if (!openKalender)
                {
                    // ALS de gebruiker op "Open Kalender" klikt, open dan de Kalender View en sluit deze View
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    var kalenderView = _serviceProvider.GetRequiredService<KalenderView>();
                    await Application.Current.MainPage.Navigation.PushModalAsync(kalenderView);
                }
                return;
            }
            
            // Als iemand door wil gaan met reserveren voor een staanplaats is gekozen, blokkeren.
            if (GeselecteerdeStaanplaats == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Geen plaats gekozen",
                    "Selecteer alstublieft een staanplaats op de kaart.",
                    "OK");
                return;
            }

            // Wel nog ff periode checken op geldigheid, ook al zou dit al in de Kalender gebeurd moeten zijn
            if (!_reservatieDataService.IsValidPeriod())
            {
                await Application.Current.MainPage.DisplayAlert("Ongeldig", "De geselecteerde periode is niet geldig.", "OK");
                return;
            }

            // Dan, als alles ok is, het veld opslaan en doorgaan naar overzicht
            _reservatieDataService.SelectedVeld = Veld;
            _reservatieDataService.SelectedStaanplaats = GeselecteerdeStaanplaats;

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