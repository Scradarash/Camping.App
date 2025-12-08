using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Models;

namespace Camping.Core.Services
{
    public class ReserveringService : IReserveringService
    {
        private readonly IReserveringRepository _repository;

        public ReserveringService(IReserveringRepository repository)
        {
            _repository = repository;
        }

        public void MaakReservering(
            DateTime startDatum,
            DateTime eindDatum,
            Veld veld,
            Staanplaats staanplaats,
            Accommodatie accommodatie)
        {
            if (veld == null) throw new ArgumentNullException(nameof(veld));
            if (staanplaats == null) throw new ArgumentNullException(nameof(staanplaats));
            if (accommodatie == null) throw new ArgumentNullException(nameof(accommodatie));

            var reservering = new Reservering
            {
                StartDatum = startDatum,
                EindDatum = eindDatum,
                VeldId = veld.id,
                StaanplaatsId = staanplaats.id,
                AccommodatieId = accommodatie.Id
            };

            _repository.Add(reservering);
        }
    }
}
