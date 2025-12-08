using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using MySqlConnector;

namespace Camping.Core.Data.Repositories
{
    public class ReserveringRepository : IReserveringRepository
    {
        private readonly DbConnection _db;

        public ReserveringRepository(DbConnection db)
        {
            _db = db;
        }

        public void Add(Reservering reservering)
        {
            // Connectie maken via de helper in plaats van direct in de repository (fucking Peter jonge)
            using var connection = _db.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();

            // Prijs voor nu nog niet meegenomen, gasten en totaalprijs worden later toegevoegd.
            // Voor nu alle prijzen op 0.00 zetten en reserveringhouder op id=1
            command.CommandText = @"
                INSERT INTO reserveringen
                    (aankomstdatum, vertrekdatum, staanplaats_id, accommodatie_type_id, reserveringhouder_id, totaal_prijs)
                VALUES
                    (@start, @end, @staanplaatsId, @accommodatieId, 1, 0.00);";

            command.Parameters.Add("@start", MySqlDbType.Date).Value = reservering.StartDatum;
            command.Parameters.Add("@end", MySqlDbType.Date).Value = reservering.EindDatum;
            command.Parameters.Add("@staanplaatsId", MySqlDbType.Int32).Value = reservering.StaanplaatsId;
            command.Parameters.Add("@accommodatieId", MySqlDbType.Int32).Value = reservering.AccommodatieId;

            command.ExecuteNonQuery();

            // Het ID van de nieuwe reservering opslaan in het object
            reservering.Id = (int)command.LastInsertedId;
        }
    }
}