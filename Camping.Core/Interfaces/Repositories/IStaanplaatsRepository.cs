using Camping.Core.Models;

namespace Camping.Core.Interfaces.Repositories
{
    public interface IStaanplaatsRepository
    {
        IEnumerable<Staanplaats> GetAll();
        IEnumerable<Staanplaats> GetByVeldId(int veldId);
        Staanplaats? GetById(int id);
        void UpdateStatus(int id, string nieuweStatus);
    }
}
