using Camping.Core.Models;

namespace Camping.Core.Interfaces.Repositories
{
    public interface IAccommodatieRepository
    {
        IEnumerable<Accommodatie> GetAll();

        //Accommodaties teruggeven die ten minste op één staanplaats van dit veld toegestaan zijn
        IEnumerable<Accommodatie> GetByVeldId(int veldId);
    }
}