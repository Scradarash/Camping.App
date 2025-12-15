using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IReserveringService
    {
        Task MaakReserveringAsync(
            DateTime startDatum,
            DateTime eindDatum,
            Veld veld,
            Staanplaats staanplaats,
            Accommodatie accommodatie);
    }
}
