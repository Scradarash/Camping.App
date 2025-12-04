using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Camping.Core.Data.Repositories;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;

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
    }
}
