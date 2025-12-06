using Camping.App.Views;
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
        private readonly IAccommodatieService _accommodatieService;
        private readonly IServiceProvider _serviceProvider; // Nodig om de Kalender te kunnen openen

        [ObservableProperty]
        private Veld? veld;

        [ObservableProperty]
        private string? periodeTekst;

        public ObservableCollection<Accommodatie> GeschikteAccommodaties { get; } = new();

        // ServiceProvider toegevoegd aan constructor
        public VeldDetailViewModel(IReservatieDataService reservatieDataService, IAccommodatieService accommodatieService, IServiceProvider serviceProvider)
        {
            _reservatieDataService = reservatieDataService;
            _accommodatieService = accommodatieService;
            _serviceProvider = serviceProvider;
        }

        public void Initialize(Veld geselecteerdeVeld)
        {
            // Veldinfo moet inzichtelijk zijn, ook als er geen datum is geselecteerd
            Veld = geselecteerdeVeld;
            PeriodeTekst = "Nog geen datum geselecteerd";

            // Als er al een periode is geselecteerd, dan wordt die hier getoond bovenaan
            if (_reservatieDataService.StartDate.HasValue && _reservatieDataService.EndDate.HasValue)
            {
                PeriodeTekst = $"{_reservatieDataService.StartDate:dd-MM-yyyy} - {_reservatieDataService.EndDate:dd-MM-yyyy}";
            }

            // 3. Probeer accommodaties te laden, maar crash niet als dit mislukt (bijv. door missende datum)
            try
            {
                GeschikteAccommodaties.Clear();
                // Als de service datums nodig heeft, kan dit fout gaan als ze null zijn.
                // Daarom try-catch gebruiken. Dit is een tijdelijke oplossing, idealiter zou de service zelf beter met null-waarden om moeten gaan.
                var lijst = _accommodatieService.GetGeschikteAccommodaties((Veld)Veld);

                foreach (var item in lijst)
                {
                    GeschikteAccommodaties.Add(item);
                }
            }
            catch (Exception)
            {
                // Een catch zodat het scherm niet crasht bij fouten, zoals missende periode.
            }
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
                    "Open Kalender", "Annuleer");

                if (openKalender)
                {
                    // ALS de gebruiker op "Open Kalender" klikt, open dan de Kalender View en sluit deze View
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    var kalenderView = _serviceProvider.GetRequiredService<KalenderView>();
                    await Application.Current.MainPage.Navigation.PushModalAsync(kalenderView);
                }
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