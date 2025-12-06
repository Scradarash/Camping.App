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
            // Contoleren of er een datum geselecteerd is
            if (_startDatum == null || _eindDatum == null)
                return string.Empty;

            // Dan controleren of die datum ook binnen het huidige jaar valt
            if (_startDatum.Value.Year != DateTime.Now.Year || _eindDatum.Value.Year != DateTime.Now.Year)
                return "Reserveren is alleen mogelijk binnen het huidige kalenderjaar.";

            // Zo ja, opslaan
            _reservatieService.StartDate = _startDatum;
            _reservatieService.EndDate = _eindDatum;

            return string.Empty;
        }
    }
}