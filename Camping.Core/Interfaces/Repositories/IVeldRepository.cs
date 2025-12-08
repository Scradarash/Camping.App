using Camping.Core.Models;

namespace Camping.Core.Interfaces.Repositories;

public interface IVeldRepository
{
    IEnumerable<Veld> GetAll();
}
