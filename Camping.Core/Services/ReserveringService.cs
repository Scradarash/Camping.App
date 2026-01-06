using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Models;

namespace Camping.Core.Services
{
    public class ReserveringService : IReserveringService
    {
        private readonly IGastRepository _gastRepository;
        private readonly IReserveringRepository _reserveringRepository;
        private readonly IReservatieDataService _data;
        public ReserveringService(
            IGastRepository gastRepository,
            IReserveringRepository reserveringRepository,
            IReservatieDataService reservatieDataService
            )
        {
            _gastRepository = gastRepository;
            _reserveringRepository = reserveringRepository;
            _data = reservatieDataService;
        }

        // 'gast id' koppelen aan 'reserveringshouder id'.
        // De gast die de reservering maakt is de reserveringshouder
        public async Task MaakReserveringAsync(
            DateTime startDatum,
            DateTime eindDatum,
            Veld veld,
            Staanplaats staanplaats,
            Accommodatie accommodatie,
            bool kiestStroom,
            bool kiestWater,
            decimal totaalPrijs)
        {
            if (string.IsNullOrWhiteSpace(_data.Emailadres) || string.IsNullOrWhiteSpace(_data.Naam) ||
                !_data.Geboortedatum.HasValue || string.IsNullOrWhiteSpace(_data.Telefoonnummer))
            {
                throw new InvalidOperationException("De gastgegevens van de reserveringshouder zijn nog niet compleet.");
            }

            var email = _data.Emailadres.Trim();
            var bestaandeGast = await _gastRepository.GetByEmailAsync(email);

            // Controleren of gast al bestaat en anders maak een nieuw Gast object aan.
            int reserveringhouderId;
            if (bestaandeGast != null)
            {
                reserveringhouderId = bestaandeGast.Id;
            }
            else
            {
                var nieuweGast = new Gast
                {
                    Naam = _data.Naam.Trim(),
                    Geboortedatum = DateOnly.FromDateTime(_data.Geboortedatum.Value),
                    Email = email,
                    Telefoon = _data.Telefoonnummer.Trim()
                };

                reserveringhouderId = await _gastRepository.AddAsync(nieuweGast);
            }

            var nieuweReservering = new Reservering
            {
                StartDatum = startDatum,
                EindDatum = eindDatum,
                VeldId = veld.id,
                StaanplaatsId = staanplaats.id,
                AccommodatieId = accommodatie.Id,
                KiestStroom = kiestStroom,
                KiestWater = kiestWater,
                TotaalPrijs = totaalPrijs
            };

            // Reservering met bijhorende gast toevoegen aan reserveringRepository
            await _reserveringRepository.AddAsync(nieuweReservering, reserveringhouderId);
        }
    }
}