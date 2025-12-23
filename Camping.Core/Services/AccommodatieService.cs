using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Models;

namespace Camping.Core.Services
{
    public class AccommodatieService : IAccommodatieService
    {
        private readonly IAccommodatieRepository _accommodatieRepository;

        public AccommodatieService(IAccommodatieRepository accommodatieRepository)
        {
            _accommodatieRepository = accommodatieRepository;
        }

        public IEnumerable<Accommodatie> GetGeschikteAccommodaties(Veld veld)
        {
            return _accommodatieRepository.GetByVeldId(veld.id);
        }

        public IEnumerable<Accommodatie> GetGeschikteAccommodatiesVoorStaanplaats(int staanplaatsId)
        {
            return _accommodatieRepository.GetByStaanplaatsId(staanplaatsId);
        }
    }
}