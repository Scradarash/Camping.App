using Camping.Core.Models;
using System.Collections.ObjectModel;

namespace Camping.Core.Interfaces.Services
{
    public interface IReserveringService
    {
        Task MaakReserveringAsync(
            DateTime startDatum,
            DateTime eindDatum,
            Veld veld,
            Staanplaats staanplaats,
            Accommodatie accommodatie,
            bool kiestStroom,  
            bool kiestWater,    
            decimal totaalPrijs,
            ObservableCollection<Gast> Gastenlijst
            );
    }
}
