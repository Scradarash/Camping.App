using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Models;

namespace Camping.Core.Services
{
    public class AccommodatieService : IAccommodatieService
    {
        private readonly IAccommodatieRepository _repository;

        public AccommodatieService(IAccommodatieRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Accommodatie> GetGeschikteAccommodaties(Veld veld)
        {
            // Veiligheidscheck
            if (veld == null) return Enumerable.Empty<Accommodatie>();

            // DB-gedreven: wat er daadwerkelijk op de staanplaatsen in dit veld is toegestaan.
            return _repository.GetByVeldId(veld.id);
        }
    }
}
