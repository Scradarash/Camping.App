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

        public IEnumerable<Accommodatie> GetGeschikteAccommodaties(Staanplaats staanplaats)
        {
            var alleAccommodaties = _repository.GetAll();
            var geschikteLijst = new List<Accommodatie>();

            // Veiligheidscheck
            if (staanplaats == null) return geschikteLijst;

            foreach (var acc in alleAccommodaties)
            {
                bool toevoegen = false;

                if (staanplaats.Name.Contains("Trekkersveld"))
                {
                    if (acc.Name == "Tent") toevoegen = true;
                }
                else if (staanplaats.Name.Contains("Chaletveld"))
                {
                    if (acc.Name == "Chalet") toevoegen = true;
                }
                else
                {
                    // Overige velden: Alles behalve Chalet
                    if (acc.Name != "Chalet") toevoegen = true;
                }

                if (toevoegen)
                {
                    geschikteLijst.Add(acc);
                }
            }

            return geschikteLijst;
        }
    }
}