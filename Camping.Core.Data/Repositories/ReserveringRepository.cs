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

        public async Task AddAsync(Reservering reservering, int reserveringhouderId)
        {
            // Connectie object aanmaken
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();

            // Prijs voor nu nog niet meegenomen, gasten en totaalprijs worden later toegevoegd.
            // Voor nu alle prijzen op 0.00 zetten en reserveringhouder op id=1
            command.CommandText = @"
                INSERT INTO reserveringen
                    (aankomstdatum, vertrekdatum, staanplaats_id, accommodatie_type_id, reserveringhouder_id, totaal_prijs)
                VALUES
                    (@start, @end, @staanplaatsId, @accommodatieId, @houderId, 0.00);";

            command.Parameters.Add("@start", MySqlDbType.Date).Value = reservering.StartDatum;
            command.Parameters.Add("@end", MySqlDbType.Date).Value = reservering.EindDatum;
            command.Parameters.Add("@staanplaatsId", MySqlDbType.Int32).Value = reservering.StaanplaatsId;
            command.Parameters.Add("@accommodatieId", MySqlDbType.Int32).Value = reservering.AccommodatieId;
            command.Parameters.Add("@houderId", MySqlDbType.Int32).Value = reserveringhouderId;

            await command.ExecuteNonQueryAsync();    

            // Applicatie houd bij welke Id de DB heeft toegekend en slaat het op in het object.
            reservering.Id = (int)command.LastInsertedId;
        }
    }
}