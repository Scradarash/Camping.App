using Camping.Core.Interfaces.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Calendar;

namespace Camping.App.ViewModels
{
    public partial class KalenderViewModel : ObservableObject
    {
        private DateTime? _startDatum;
        private DateTime? _eindDatum;

        private readonly IReservatieDataService _reservatieService;

        public KalenderViewModel(IReservatieDataService reservatieService)
        {
            _reservatieService = reservatieService;

            // Minimum datum ingesteld op vandaag, dus er kan niet in het verleden geboekt worden (door binding in view)
            MinDate = DateTime.Now.Date;
            // En de maximum datum stellen we in op het einde van het huidige jaar
            MaxDate = new DateTime(DateTime.Now.Year, 12, 31);
        }

        // Properties voor de kalender restricties
        public DateTime MinDate { get; }
        public DateTime MaxDate { get; }

        public DateTime? StartDatum => _startDatum;
        public DateTime? EndDatum => _eindDatum;

        public void UpdateRange(CalendarDateRange range)
        {
            _startDatum = range?.StartDate;
            _eindDatum = range?.EndDate;
        }

        // Veranderd van bool naar string voor betere foutmeldingen (error opslaan in string, tonen in view.xaml.cs)
        public string OpslaanDatum()
        {
            // Checken of er iets is geselecteerd, zo niet, gewoon lege string teruggeven.
            if (_startDatum == null || _eindDatum == null)
            {
                return string.Empty;
            }

            // Als er wel iets is geselecteerd, valideren we de input (check service voor details)
            string error = _reservatieService.ValidateInput(_startDatum, _eindDatum);

            // Als er een error is (dus string = niet leeg) error returnen.
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }

            // 3. Als we hier zijn is alles geldig -> Opslaan
            _reservatieService.StartDate = _startDatum;
            _reservatieService.EndDate = _eindDatum;

            return string.Empty;
        }
    }
}