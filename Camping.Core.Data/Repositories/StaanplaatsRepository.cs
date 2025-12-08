using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Camping.Core.Data.Repositories
{
    public class StaanplaatsRepository : IStaanplaatsRepository
    {
        private readonly DbConnection _db;

        public StaanplaatsRepository(DbConnection db)
        {
            _db = db;
        }

        // Haal alle staanplaatsen op die bij een specifiek veld horen
        public IEnumerable<Staanplaats> GetByVeldId(int veldId)
        {
            var staanplaatsen = new List<Staanplaats>();

            using var connection = _db.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            // SQL Query uitleg:
            // Selecteer de staanplaatsen (s) en hun bijbehorende accommodatie types (a) via de koppeltabel (sa)
            // We joinen staanplaatsen aan de koppeltabel en vervolgens de koppeltabel aan de accommodatie types
            // Die bij een bepaald veld horen (WHERE s.veld_id = @veldId) (het geselecteerde veld)
            // Accommodatie types worden samengevoegd in 1 kolom per staanplaats via GROUP_CONCAT, om deze te tonen
            command.CommandText = @"
                SELECT s.id, s.veld_id, 
                       GROUP_CONCAT(a.naam SEPARATOR ', ') as types
                FROM staanplaatsen s
                LEFT JOIN staanplaats_accommodatietypes sa ON s.id = sa.staanplaats_id
                LEFT JOIN accommodatie_types a ON sa.accommodatie_type_id = a.id
                WHERE s.veld_id = @veldId
                GROUP BY s.id, s.veld_id";

            command.Parameters.AddWithValue("@veldId", veldId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                staanplaatsen.Add(new Staanplaats
                {
                    id = reader.GetInt32("id"),
                    VeldId = reader.GetInt32("veld_id"),

                    // Als er geen types gekoppeld zijn, toon 'Onbekend', anders de lijst met types (dit is AI, ik snap t ook niet helemaal)
                    AccommodatieType = reader.IsDBNull(reader.GetOrdinal("types"))
                                       ? "Onbekend"
                                       : reader.GetString("types"),

                    Status = "Beschikbaar", // Vooralsnog altijd beschikbaar, tot we reserveringen verder hebben uitgewerkt

                    // Deze kolommen zijn uit de DB gehaald, dus zetten we ze voor nu op true
                    // Zodat de icoontjes in de UI zichtbaar blijven
                    HeeftStroom = true,
                    HeeftWater = true
                });
            }

            return staanplaatsen;
        }

        // Haal 1 specifieke staanplaats op
        public Staanplaats? GetById(int id)
        {
            using var connection = _db.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            // Dezelfde query als hierboven, maar dan specifiek voor 1 ID
            command.CommandText = @"
                SELECT s.id, s.veld_id, 
                       GROUP_CONCAT(a.naam SEPARATOR ', ') as types
                FROM staanplaatsen s
                LEFT JOIN staanplaats_accommodatietypes sa ON s.id = sa.staanplaats_id
                LEFT JOIN accommodatie_types a ON sa.accommodatie_type_id = a.id
                WHERE s.id = @id
                GROUP BY s.id, s.veld_id";

            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Staanplaats
                {
                    id = reader.GetInt32("id"),
                    VeldId = reader.GetInt32("veld_id"),
                    AccommodatieType = reader.IsDBNull(reader.GetOrdinal("types"))
                                       ? "Onbekend"
                                       : reader.GetString("types"),
                    Status = "Beschikbaar",
                    HeeftStroom = true,
                    HeeftWater = true
                };
            }
            return null;
        }

        public IEnumerable<Staanplaats> GetAll()
        {
            throw new NotImplementedException();
            // return new List<Staanplaats>();
        }

        public void UpdateStatus(int id, string nieuweStatus)
        {
            throw new NotImplementedException();
        }
    }
}