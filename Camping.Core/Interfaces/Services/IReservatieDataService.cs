using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IReservatieDataService
    {

        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }

        Veld? SelectedVeld { get; set; }

        bool IsValidPeriod();
    }
}
