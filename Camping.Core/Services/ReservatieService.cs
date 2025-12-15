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
        public async Task MaakReservering()
        {
            var gast = await _gastRepository.GetByEmailAsync(_data.Emailadres!);

            // nieuw gast object aanmaken als gast == null (er is geen gast)
            int gastId;
            if (gast == null)
            {
                gast = new Gast
                {
                    Naam = _data.Naam!,
                    Geboortedatum = DateTime(_data.Geboortedatum!.Value),
                    Email = _data.Emailadres!,
                    Telefoon = _data.Telefoonnummer!
                };

                gastId = await _gastRepository.AddAsync(gast);
            }

            else
            {
                gastId = gast.Id;
            }
            
            // Nieuw reservering object aanmaken 
            var reservering = new Reservering
            {
                StartDatum = _data.StartDate!.Value,
                EindDatum = _data.EndDate!.Value,
                VeldId = _data.SelectedVeld!.id,
                StaanplaatsId = _data.SelectedStaanplaats!.id,
                AccommodatieId = _data.SelectedAccommodatie!.Id
            };

            await _reserveringRepository.AddAsync(reservering, gastId);
        }
    }
}
