using Camping.Core.Models;
using Camping.Core.Interfaces.Services;

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

            // Check of de einddatum na de startdatum ligt
            if (end <= start)
                return "De vertrekdatum moet na de aankomstdatum liggen.";

            // Check of de datums binnen het huidige kalenderjaar liggen
            if (start.Value.Year != DateTime.Now.Year || end.Value.Year != DateTime.Now.Year)
                return "Reserveren is alleen mogelijk binnen het huidige kalenderjaar.";

            // Als alle checks slagen, empty string, dus geen foutmelding
            return string.Empty;
        }
    }
}
