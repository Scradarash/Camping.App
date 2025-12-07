using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Camping.Core.Data.Repositories
{
    public class StaanplaatsRepository : IStaanplaatsRepository
    {
        private readonly List<Staanplaats> _staanplaatsen;

        public StaanplaatsRepository()
        {
            _staanplaatsen = new List<Staanplaats>();
            InitializeStaanplaatsen();
        }

        private void InitializeStaanplaatsen()
        {
            // Startende ID voor staanplaatsen
            int currentId = 1;
            // Maken geen klikbare staanplaatsen voor groepsveld want die zijn dus niet reserveerbaar
            currentId = AddPlekken(currentId, 2, 12, "Tent", false, false);
            currentId = AddPlekken(currentId, 3, 12, "Chalet", true, true);
            currentId = AddPlekken(currentId, 4, 8, "Caravan", true, false);
            currentId = AddPlekken(currentId, 5, 9, "Camper", true, true);
        }
        // Deze methode voegt een aantal plekken toe en retourneert de volgende beschikbare ID
        // Dus iedere keer dat initializeStaanplaatsen wordt aangeroepen, start het met de juiste ID
        // Hierdoor begint niet iedere veld opnieuw bij 1
        private int AddPlekken(int startId, int veldId, int aantal, string type, bool stroom, bool water)
        {
            for (int i = 0; i < aantal; i++)
            {
                _staanplaatsen.Add(new Staanplaats
                {
                    id = startId,
                    VeldId = veldId,
                    AccommodatieType = type,
                    HeeftStroom = stroom,
                    HeeftWater = water,
                    Status = "Beschikbaar"
                });
                startId++;
            }
            return startId;
        }

        // Methode om alle staanplaatsen op te halen
        public IEnumerable<Staanplaats> GetAll() => _staanplaatsen;
        // Methode om staanplaatsen per veld op te halen
        public IEnumerable<Staanplaats> GetByVeldId(int veldId)
        {
            return _staanplaatsen.Where(p => p.VeldId == veldId);
        }
        // Methode om een staanplaats op te halen via ID
        public Staanplaats? GetById(int id)
        {
            return _staanplaatsen.FirstOrDefault(p => p.id == id);
        }
        // Methode om de status van een staanplaats bij te werken (nog niet gebruikt)
        public void UpdateStatus(int id, string nieuweStatus)
        {
            var plek = GetById(id);
            if (plek != null)
            {
                plek.Status = nieuweStatus;
            }
        }
    }
}