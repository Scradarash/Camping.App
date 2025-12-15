using Camping.Core.Models;

namespace Camping.Core.Interfaces.Repositories
{
    public interface IReserveringRepository
    {
        Task AddAsync(Reservering reservering, int reserveringhouderId);
    }
}
