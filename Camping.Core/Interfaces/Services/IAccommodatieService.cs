using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IAccommodatieService
    {
        // Geeft alleen de accommodaties terug die toegestaan zijn op het gegeven veld
        IEnumerable<Accommodatie> GetGeschikteAccommodaties(Veld veld);
    }
}