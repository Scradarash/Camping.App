using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IAccommodatieService
    {
        // Geeft alleen de accommodaties terug die toegestaan zijn op de gegeven staanplaats
        IEnumerable<Accommodatie> GetGeschikteAccommodaties(Staanplaats staanplaats);
    }
}