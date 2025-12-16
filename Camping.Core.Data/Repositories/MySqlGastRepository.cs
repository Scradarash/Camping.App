using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Camping.Core.Data.Repositories;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using Camping.Core.Data.Helpers;

namespace Camping.Core.Data.Repositories
{
    public class MySqlGastRepository : IGastRepository
    {
        private readonly DbConnection _db;
        
        // Instantieert connectie met database, elke methode in repository kan MySql connecties maken
        public MySqlGastRepository(DbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Gast>> GetAllAsync()
        {
            var result = new List<Gast>();

            // Connectie openen en automatisch sluiten van de DB via 'await using'
            await using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            // Select query uitvoeren op 'gasten'om gegevens op te halen uit DB
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT id, naam, geboortedatum, email, telefoon FROM gasten;";

            // Alle gast gegevens uit de DB per rij lezen en in een lijst opslaan (result)
            // In 'while' loop, DB entiteiten mappen naar models in 'Gast'
            await using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                result.Add(
                    new Gast
                    {
                        Id = reader.GetInt32("id"),
                        Naam = reader.GetString("naam"),
                        Geboortedatum = DateOnly.FromDateTime(reader.GetDateTime("geboortedatum")),
                        Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                        Telefoon = reader.IsDBNull(reader.GetOrdinal("telefoon")) ? null : reader.GetString("telefoon")
                    });
            }

            return result;
        }

        // Zoeken of gast al bestaat in de database en daarvan een object aanmaken voor de View
        public async Task<Gast?> GetByEmailAsync(string email)
        {
            await using var connection = _db.CreateConnection();

            var command = connection.CreateCommand();

            command.CommandText = @"
            SELECT id, naam, geboortedatum, email, telefoon
            FROM gasten 
            WHERE email = @email";

            command.Parameters.AddWithValue("@email", email);

            using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return null;    // Als er geen rij is gevonden in DB geef null terug
            }

            return new Gast
            {
                Id = reader.GetInt32("id"),
                Naam = reader.GetString("naam"),
                Geboortedatum = DateOnly.FromDateTime(reader.GetDateTime("geboortedatum")),
                Email = reader.GetString("email"),
                Telefoon = reader.GetString("telefoon")
            };
        }

        // Gast toevoegen aan database als de gast nog niet bestaat
        public async Task<int> AddAsync(Gast gast)
        {
            await using var connection = _db.CreateConnection();
            var command = connection.CreateCommand();

            // Query voor opslaan data in DB, Selecteer laatste Id 
            command.CommandText = @"
                INSERT INTO gasten (naam, geboortedatum, email, telefoon)
                VALUES (@naam, @geboortedatum, @email, @telefoon);
                SELECT LAST_INSERT_ID();";

            // Vul parameters met waardes van object in de query
            command.Parameters.AddWithValue("@naam", gast.Naam);
            command.Parameters.AddWithValue("@geboortedatm", gast.Geboortedatum);
            command.Parameters.AddWithValue("@email", gast.Email);
            command.Parameters.AddWithValue("@telefoon", gast.Telefoon);

            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
    }
}
