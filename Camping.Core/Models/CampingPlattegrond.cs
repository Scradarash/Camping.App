using System.Collections.Generic;
using System.Linq;
using Camping.Core.Models;
using Camping.Core.Interfaces.Repositories;

namespace Camping.Core.Models
{
    public class CampingPlattegrond
    {
        public List<Veld> Velden { get; }

        public CampingPlattegrond(IVeldRepository repo)
        {
            Velden = repo.GetAll().ToList();
        }
    }
}
