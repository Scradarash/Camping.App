using Camping.Core.Models;
using Camping.Core.Interfaces.Services;

namespace Camping.Core.Services
{
    public class ReservatieDataService : IReservatieDataService
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Veld? SelectedVeld { get; set; }

        //Nodig voor later controleren op correctheid datum
        public bool IsValidPeriod()
        {
            return StartDate != null && EndDate != null && EndDate > StartDate;
        }
    }
}
