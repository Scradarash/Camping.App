using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;

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
        public IEnumerable<Staanplaats> GetByVeldId(int veldId, DateTime? start = null, DateTime? end = null)
        {
            var staanplaatsen = new List<Staanplaats>();

            using var connection = _db.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            // Nieuwe logica voor beschikbaarheid:
            // We bouwen een subquery die telt hoeveel reserveringen er zijn die overlappen met de gekozen periode.
            // Als er geen datums zijn geselecteerd, gaan we uit van 0 (alles beschikbaar).
            string bezetCheckSql = BuildBezetCheckSql(start, end);

            // SQL Query uitleg:
            // Selecteer staanplaatsen (s) en hun bijbehorende accommodatie types (a) via de koppeltabel (sa)
            // Accommodatie types worden samengevoegd in 1 kolom per staanplaats via GROUP_CONCAT, om deze te tonen
            // We selecteren ook de bezetCheck subquery die telt hoeveel boekingen er zijn in de gekozen periode
            command.CommandText = $@"
                SELECT 
                    s.id, 
                    s.veld_id,
                    s.prijs,
                    s.aantal_gasten,
                    GROUP_CONCAT(DISTINCT a.naam ORDER BY a.naam SEPARATOR ', ') as types,
                    {bezetCheckSql} as aantal_boekingen
                FROM staanplaatsen s
                LEFT JOIN staanplaats_accommodatietypes sa ON s.id = sa.staanplaats_id
                LEFT JOIN accommodatie_types a ON sa.accommodatie_type_id = a.id
                WHERE s.veld_id = @veldId
                GROUP BY s.id, s.veld_id, s.prijs, s.aantal_gasten
                ORDER BY s.id;";

            command.Parameters.AddWithValue("@veldId", veldId);

            // Voeg datum parameters alleen toe als ze zijn meegegeven
            AddDateParametersIfNeeded(command, start, end);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                bool isBezet = reader.GetInt64("aantal_boekingen") > 0;
                staanplaatsen.Add(MapStaanplaats(reader, isBezet));
            }

            return staanplaatsen;
        }

        private static string BuildBezetCheckSql(DateTime? start, DateTime? end)
        {
            if (start == null || end == null)
                return "0";

            // Overlap: (BestaandStart < NieuwEind) EN (BestaandEind > NieuwStart)
            return @"(SELECT COUNT(*) 
                      FROM reserveringen r 
                      WHERE r.staanplaats_id = s.id 
                        AND r.aankomstdatum < @end 
                        AND r.vertrekdatum > @start)";
        }

        private static void AddDateParametersIfNeeded(dynamic command, DateTime? start, DateTime? end)
        {
            if (start == null || end == null)
                return;

            command.Parameters.AddWithValue("@start", start.Value.Date);
            command.Parameters.AddWithValue("@end", end.Value.Date);
        }

        private static Staanplaats MapStaanplaats(dynamic reader, bool isBezet)
        {
            return new Staanplaats
            {
                id = reader.GetInt32("id"),
                VeldId = reader.GetInt32("veld_id"),
                Prijs = reader.GetDecimal("prijs"),
                AantalGasten = reader.GetInt32("aantal_gasten"),

                // Als er geen types gekoppeld zijn, toon 'Onbekend', anders de lijst met types
                AccommodatieType = reader.IsDBNull(reader.GetOrdinal("types"))
                    ? "Onbekend"
                    : reader.GetString("types"),

                // Status wordt nu bepaald door de database check
                Status = isBezet ? "Bezet" : "Beschikbaar",

                // Hier kun je later echte DB kolommen of een aparte tabel voor gebruiken.
                HeeftStroom = true,
                HeeftWater = true
            };
        }

        // Haal 1 specifieke staanplaats op
        public Staanplaats? GetById(int id)
        {
            using var connection = _db.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            // Dezelfde query als hierboven, maar dan specifiek voor 1 ID
            command.CommandText = @"
                SELECT 
                    s.id, 
                    s.veld_id,
                    s.prijs,
                    s.aantal_gasten,
                    GROUP_CONCAT(DISTINCT a.naam ORDER BY a.naam SEPARATOR ', ') as types
                FROM staanplaatsen s
                LEFT JOIN staanplaats_accommodatietypes sa ON s.id = sa.staanplaats_id
                LEFT JOIN accommodatie_types a ON sa.accommodatie_type_id = a.id
                WHERE s.id = @id
                GROUP BY s.id, s.veld_id, s.prijs, s.aantal_gasten
                ORDER BY s.id;";

            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Staanplaats
                {
                    id = reader.GetInt32("id"),
                    VeldId = reader.GetInt32("veld_id"),
                    Prijs = reader.GetDecimal("prijs"),
                    AantalGasten = reader.GetInt32("aantal_gasten"),
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