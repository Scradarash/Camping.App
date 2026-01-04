using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IAccommodatieService
    {
        IEnumerable<Accommodatie> GetGeschikteAccommodaties(Veld veld);

        IEnumerable<Accommodatie> GetGeschikteAccommodatiesVoorStaanplaats(int staanplaatsId);
    }
}
