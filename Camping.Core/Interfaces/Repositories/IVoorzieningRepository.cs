using Camping.Core.Models;

namespace Camping.Core.Interfaces.Repositories
{
    public interface IVoorzieningRepository
    {
        IEnumerable<Voorziening> GetAll();
        Voorziening? GetByNaam(string naam);
    }
}
