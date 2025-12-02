using Camping.Core.Models;

namespace Camping.Core.Interfaces.Repositories;

public interface IStaanplaatsRepository
{
    IEnumerable<Staanplaats> GetAll();
}
