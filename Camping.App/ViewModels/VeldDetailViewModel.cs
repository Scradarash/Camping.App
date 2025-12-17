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
        private Veld? _veld;

        [ObservableProperty]
        private string? _periodText;

        // ObservableProperty voor het tonen van gekozen staanplaats.
        [ObservableProperty]
        private string _selectedStaanplaatsText = "Kies een plaats op de kaart";

        [ObservableProperty]
        private string _reserveButtonText = "Reserveer";

        // Deze wordt gebruikt door view om reserveer knop te disablen bij groepsveld
        [ObservableProperty]
        private bool _isReserveEnabled = true;

        public ObservableCollection<Staanplaats> Staanplaatsen { get; } = new();

        [ObservableProperty]
        private Staanplaats? _selectedStaanplaats;

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
            PeriodText = "Nog geen datum geselecteerd";

            // GeselecteerdeStaanplaats leegmaken
            SelectedStaanplaats = null;
            SelectedStaanplaatsText = "Kies een staanplaats op de kaart";

            // Als er al een periode is geselecteerd, dan wordt die hier getoond bovenaan
            if (_reservatieDataService.StartDate.HasValue && _reservatieDataService.EndDate.HasValue)
            {
                PeriodText = $"{_reservatieDataService.StartDate:dd-MM-yyyy} - {_reservatieDataService.EndDate:dd-MM-yyyy}";
            }

            // Als het groepsveld geselecteerd is, dan passen we de teksten en button aan
            if (Veld.Name.Contains("Groepsveld", StringComparison.OrdinalIgnoreCase))
            {
                ReserveButtonText = "Neem contact op met de camping!";
                // Geselecteerde staanplaats tekst leeg maken want is irrelevant, je kan niet reserveren via de app
                SelectedStaanplaatsText = "";
                // Zelfde geldt dus voor de periode tekst
                PeriodText = "";
                IsReserveEnabled = false;
            }
            else
            {
                ReserveButtonText = "Reserveer";
                IsReserveEnabled = true;
            }

            LoadStaanplaatsen();
        }

        private void LoadStaanplaatsen()
        {
            Staanplaatsen.Clear();
            // Haal alle staanplaatsen op voor dit veld
            IEnumerable<Staanplaats> plekken = _staanplaatsRepository.GetByVeldId(
                Veld.id,
                _reservatieDataService.StartDate,
                _reservatieDataService.EndDate
            );

            foreach (Staanplaats plek in plekken)
            {
                Staanplaatsen.Add(plek);
            }
        }

        [RelayCommand]
        //Klikken op een staanplaats
        private void SelectStaanplaats(Staanplaats plek)
        {
            SelectedStaanplaats = plek;
            _reservatieDataService.SelectedStaanplaats = plek;
            SelectedStaanplaatsText = $"Geselecteerde plaats: {plek.id}";
        }

        [RelayCommand]
        private async Task Reserve()
        {
            try
            {
                // Pas bij reserveren de checks uitvoeren, zodat de gebruiker eerst de veldinfo kan bekijken
                if (!await ValidatePeriod())
                    return;

                // Als iemand door wil gaan met reserveren voor een staanplaats is gekozen, blokkeren.
                if (SelectedStaanplaats == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Geen plaats gekozen",
                        "Selecteer alstublieft een staanplaats op de kaart.",
                        "OK");
                    return;
                }

                // Dan, als alles ok is, het veld opslaan en doorgaan naar overzicht
                _reservatieDataService.SelectedVeld = Veld;
                _reservatieDataService.SelectedStaanplaats = SelectedStaanplaats;

                await Application.Current.MainPage.Navigation.PopModalAsync();
                await Shell.Current.GoToAsync(nameof(ReserveringsoverzichtView));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task<bool> ValidatePeriod()
        {
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
                    await OpenCalendar();
                }
                return false;
            }

            // Wel nog ff periode checken op geldigheid, ook al zou dit al in de Kalender gebeurd moeten zijn
            if (!_reservatieDataService.IsValidPeriod())
            {
                await Application.Current.MainPage.DisplayAlert("Ongeldig", "De geselecteerde periode is niet geldig.", "OK");
                return false;
            }

            return true;
        }

        private async Task OpenCalendar()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
            KalenderView kalenderView = _serviceProvider.GetRequiredService<KalenderView>();
            await Application.Current.MainPage.Navigation.PushModalAsync(kalenderView);
        }
    }
}