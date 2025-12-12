using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IReservatieDataService
    {
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        Veld? SelectedVeld { get; set; }
        Staanplaats? SelectedStaanplaats { get; set; }
        Accommodatie? SelectedAccommodatie { get; set; }
        string? Naam { get; set; }
        DateTime? Geboortedatum { get; set; }
        bool IsValidPeriod();
        string ValidateInput(DateTime? start, DateTime? end);
    }
}
