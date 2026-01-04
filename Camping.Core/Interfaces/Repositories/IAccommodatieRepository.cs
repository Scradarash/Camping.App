using Camping.Core.Models;

namespace Camping.Core.Interfaces.Repositories
{
    public interface IAccommodatieRepository
    {
        IEnumerable<Accommodatie> GetAll();
        IEnumerable<Accommodatie> GetByVeldId(int veldId);
        IEnumerable<Accommodatie> GetByStaanplaatsId(int staanplaatsId);
    }
}
