using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IReservatieDataService
    {

        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }

        Staanplaats? SelectedStaanplaats { get; set; }

        bool IsValidPeriod();
    }
}
