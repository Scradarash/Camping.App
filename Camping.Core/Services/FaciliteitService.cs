using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Models;

namespace Camping.Core.Services
{
    public class FaciliteitService : IFaciliteitService
    {
        private readonly IFaciliteitRepository _repository;

        public FaciliteitService(IFaciliteitRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Faciliteit> GetFaciliteiten()
        {
            return _repository.GetFaciliteiten();
        }
    }
}