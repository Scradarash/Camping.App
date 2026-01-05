using Camping.Core.Models;

namespace Camping.Core.Interfaces.Repositories
{
    public interface IAccommodatieRepository
    {
        IEnumerable<Accommodatie> GetAll();
    }
}
