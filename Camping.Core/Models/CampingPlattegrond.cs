using System.Collections.Generic;
using System.Linq;
using Camping.Core.Models;
using Camping.Core.Interfaces.Repositories;

namespace Camping.Core.Models
{
    public class CampingPlattegrond
    {
        public List<Staanplaats> Staanplaatsen { get; }

        public CampingPlattegrond(IStaanplaatsRepository repo)
        {
            Staanplaatsen = repo.GetAll().ToList();
        }
    }
}
