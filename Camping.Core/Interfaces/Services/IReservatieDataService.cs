using Camping.Core.Models;
using System.Collections.ObjectModel;

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
        string? Emailadres { get; set; }
        string? Telefoonnummer { get; set; }
        bool IsValidPeriod();
        string ValidateInput(DateTime? start, DateTime? end);

        //Tijdelijke opslag van keuzes in de Wizard
        bool KiestStroom { get; set; }
        bool KiestWater { get; set; }
        ObservableCollection<Gast> GastenLijst { get; set;}

    }
}
