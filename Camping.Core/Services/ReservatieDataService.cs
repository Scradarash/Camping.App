using Camping.Core.Models;
using Camping.Core.Interfaces.Services;
using System.Collections.ObjectModel;

namespace Camping.Core.Services
{
    public class ReservatieDataService : IReservatieDataService
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Veld? SelectedVeld { get; set; }
        public Staanplaats? SelectedStaanplaats { get; set; }
        public Accommodatie? SelectedAccommodatie { get; set; }
        public string? Naam { get; set; }
        public DateTime? Geboortedatum { get; set; }
        public string? Emailadres { get; set; }
        public string? Telefoonnummer { get; set; }
        public bool KiestStroom { get; set; }
        public bool KiestWater { get; set; }

        public ObservableCollection<Gast> GastenLijst { get; set; }

        public ReservatieDataService()
        {
            GastenLijst = new ObservableCollection<Gast>();
        }

        //Nodig voor later controleren op correctheid datum
        public bool IsValidPeriod()
        {
            return StartDate != null && EndDate != null && EndDate > StartDate;
        }

        public string ValidateInput(DateTime? start, DateTime? end)
        {
            // Check of de datums zijn ingevuld
            if (start == null || end == null)
                return "Selecteer alstublieft een start- en einddatum.";

            DateTime startDate = start.Value.Date;
            DateTime endDate = end.Value.Date;

            // Check of de einddatum na de startdatum ligt
            if (endDate <= startDate)
                return "De vertrekdatum moet na de aankomstdatum liggen.";

            // Check of er niet in het verleden geboekt wordt
            if (startDate.Year < DateTime.Today.Year)
                return "Reserveren is alleen mogelijk binnen het huidige kalenderjaar.";

            // KalenderViewModel laat boeken t/m einde van het huidige jaar, dus validatie moet hiermee matchen.
            int currentYear = DateTime.Today.Year;
            if (startDate.Year != currentYear || endDate.Year != currentYear)
                return "Reserveren is alleen mogelijk binnen het huidige kalenderjaar.";

            // Als alle checks slagen, empty string, dus geen foutmelding
            return string.Empty;
        }
    }
}
