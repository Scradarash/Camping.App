using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using MySqlConnector;

namespace Camping.Core.Data.Repositories;

public class AccommodatieRepository : IAccommodatieRepository
{
    private readonly MySqlDbExecutor _db;

    public AccommodatieRepository(MySqlDbExecutor db)
    {
        _db = db;
    }

    public IEnumerable<Accommodatie> GetAll()
    {
        const string sql = @"SELECT id, naam, prijs FROM accommodatie_types ORDER BY id;";
        return _db.Query(sql, null, Map);
    }

    public IEnumerable<Accommodatie> GetByVeldId(int veldId)
    {
        const string sql = @"
            SELECT DISTINCT a.id, a.naam, a.prijs
            FROM accommodatie_types a
            INNER JOIN staanplaats_accommodatietypes sa ON sa.accommodatie_type_id = a.id
            INNER JOIN staanplaatsen s ON s.id = sa.staanplaats_id
            WHERE s.veld_id = @veldId
            ORDER BY a.id;";

        return _db.Query(sql, cmd => cmd.Parameters.AddWithValue("@veldId", veldId), Map);
    }

    public IEnumerable<Accommodatie> GetByStaanplaatsId(int staanplaatsId)
    {
        const string sql = @"
            SELECT DISTINCT a.id, a.naam, a.prijs
            FROM accommodatie_types a
            INNER JOIN staanplaats_accommodatietypes sa 
                ON sa.accommodatie_type_id = a.id
            WHERE sa.staanplaats_id = @staanplaatsId
            ORDER BY a.id;";

        return _db.Query(sql, cmd => cmd.Parameters.AddWithValue("@staanplaatsId", staanplaatsId), Map);
    }

    private static Accommodatie Map(MySqlDataReader reader)
    {
        return new Accommodatie
        {
            Id = reader.GetInt32("id"),
            Name = reader.GetString("naam"),
            Prijs = reader.GetDecimal("prijs")
        };
    }
}
