using Camping.Core.Interfaces.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Calendar;

namespace Camping.App.ViewModels
{
    public partial class KalenderViewModel : ObservableObject
    {
        private DateTime? _startDate;
        private DateTime? _endDate;

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

        public DateTime? StartDate => _startDate;
        public DateTime? EndDate => _endDate;

        public void UpdateRange(CalendarDateRange range)
        {
            _startDate = range?.StartDate;
            _endDate = range?.EndDate;
        }

        // Veranderd van bool naar string voor betere foutmeldingen (error opslaan in string, tonen in view.xaml.cs)
        public string ValidateAndSaveDates()
        {
            // Datums naar service sturen voor validatie
            string error = _reservatieService.ValidateInput(_startDate, _endDate);

            // Als er een error is (dus string = niet leeg) error returnen.
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }

            // Na checks, datums opslaan
            _reservatieService.StartDate = _startDate;
            _reservatieService.EndDate = _endDate;

            return string.Empty;
        }
    }
}