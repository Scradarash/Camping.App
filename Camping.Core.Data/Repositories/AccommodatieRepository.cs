using Camping.Core.Models;
using Camping.Core.Interfaces.Repositories;
using System.Security.Cryptography.X509Certificates;

namespace Camping.Core.Data.Repositories;

public class AccommodatieRepository : IAccommodatieRepository
{
    
    private readonly List<Accommodatie> _accommodaties;

    //Mock data 
    public AccommodatieRepository()
    {
        _accommodaties = new List<Accommodatie>
        {
            new Accommodatie { Id = 1, Name = "Tent" },
            new Accommodatie { Id = 2, Name = "Caravan" },
            new Accommodatie { Id = 3, Name = "Camper" },
            new Accommodatie { Id = 4, Name = "Chalet" }
        };
    }

    public IEnumerable<Accommodatie> GetAll()
    {
        return _accommodaties;
    }
}
