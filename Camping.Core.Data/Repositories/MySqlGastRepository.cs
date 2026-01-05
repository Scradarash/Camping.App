using MySqlConnector;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using Camping.Core.Data.Helpers;

namespace Camping.Core.Data.Repositories
{
    public class MySqlGastRepository : IGastRepository
    {
        private readonly MySqlDbExecutor _db;

        public MySqlGastRepository(MySqlDbExecutor db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Gast>> GetAllAsync()
        {
            const string sql = @"SELECT id, naam, geboortedatum, email, telefoon FROM gasten;";
            return await _db.QueryAsync(sql, null, MapGast);
        }

        public async Task<Gast?> GetByEmailAsync(string email)
        {
            const string sql = @"
                SELECT id, naam, geboortedatum, email, telefoon
                FROM gasten 
                WHERE email = @email
                LIMIT 1;";

            return await _db.QuerySingleOrDefaultAsync(sql, cmd => cmd.Parameters.AddWithValue("@email", email), MapGast);
        }

        public async Task<int> AddAsync(Gast gast)
        {
            const string sql = @"
                INSERT INTO gasten (naam, geboortedatum, email, telefoon)
                VALUES (@naam, @geboortedatum, @email, @telefoon);
                SELECT LAST_INSERT_ID();";

            long id = await _db.ExecuteScalarAsync<long>(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@naam", gast.Naam);
                cmd.Parameters.Add("@geboortedatum", MySqlDbType.Date).Value =
                    gast.Geboortedatum.ToDateTime(TimeOnly.MinValue);
                cmd.Parameters.AddWithValue("@email", gast.Email);
                cmd.Parameters.AddWithValue("@telefoon", gast.Telefoon);
            });

            return (int)id;
        }

        private static Gast MapGast(MySqlDataReader reader)
        {
            return new Gast
            {
                Id = reader.GetInt32("id"),
                Naam = reader.GetString("naam"),
                Geboortedatum = DateOnly.FromDateTime(reader.GetDateTime("geboortedatum")),
                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email"),
                Telefoon = reader.IsDBNull(reader.GetOrdinal("telefoon")) ? string.Empty : reader.GetString("telefoon")
            };
        }
    }
}