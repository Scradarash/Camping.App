using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using MySqlConnector;

namespace Camping.Core.Data.Repositories
{
    public class VoorzieningRepository : IVoorzieningRepository
    {
        private readonly MySqlDbExecutor _db;

        public VoorzieningRepository(MySqlDbExecutor db)
        {
            _db = db;
        }

        public IEnumerable<Voorziening> GetAll()
        {
            const string sql = @"SELECT id, naam, prijs FROM voorzieningen;";
            return _db.Query(sql, null, Map);
        }

        public Voorziening? GetByNaam(string naam)
        {
            const string sql = @"
                SELECT id, naam, prijs
                FROM voorzieningen
                WHERE LOWER(naam) = LOWER(@naam)
                LIMIT 1;";

            return _db.QuerySingleOrDefault(sql, cmd => cmd.Parameters.AddWithValue("@naam", naam), Map);
        }

        private static Voorziening Map(MySqlDataReader reader)
        {
            return new Voorziening
            {
                Id = reader.GetInt32("id"),
                Naam = reader.GetString("naam"),
                Prijs = reader.GetDecimal("prijs")
            };
        }
    }
}
