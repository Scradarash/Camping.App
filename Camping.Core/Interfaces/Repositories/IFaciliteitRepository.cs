using Camping.Core.Models;

namespace Camping.Core.Interfaces.Repositories
{
    public interface IFaciliteitRepository
    {
        IEnumerable<Faciliteit> GetFaciliteiten();
    }
}