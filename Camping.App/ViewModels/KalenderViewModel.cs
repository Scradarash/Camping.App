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
        }

        // Publieke read-only properties zodat de View ze kan lezen, dit was dus een omweg om dingen CA vriendelijk te houden,
        // maar dit is mogelijk niet t beste oplossing.
        public DateTime? StartDatum => _startDatum;
        public DateTime? EndDatum => _eindDatum;

        // Deze metode wordt vanuit de KlanederView aangeroepen in OnSelectionChanged,
        // als de geselecteerde periode verandert, wordt het nieuwe periode opgeslagen
        public void UpdateRange(CalendarDateRange range)
        {
            _startDatum = range?.StartDate;
            _eindDatum = range?.EndDate;
        }

        // Logica om op te slaan. De View vraagt of he gelukt is of niet voordat die de datums opslaat,
        // om later bij niet  invoeren van een datum een melding te kunnen tonen
        public bool TrySaveDates()
        {
            if (_startDatum == null || _eindDatum == null)
                return false;

            _reservatieService.StartDate = _startDatum;
            _reservatieService.EndDate = _eindDatum;

            return true;
        }
    }
}
