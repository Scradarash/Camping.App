using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IReserveringService
    {
        void MaakReservering(
            DateTime startDatum,
            DateTime eindDatum,
            Veld veld,
            Staanplaats staanplaats,
            Accommodatie accommodatie,
            bool kiestStroom,  
            bool kiestWater,    
            decimal totaalPrijs);
    }
}
