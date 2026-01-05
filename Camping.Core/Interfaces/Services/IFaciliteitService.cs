using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IFaciliteitService
    {
        IEnumerable<Faciliteit> GetFaciliteiten();
    }
}