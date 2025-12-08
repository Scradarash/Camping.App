using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using MySqlConnector;

namespace Camping.Core.Data.Repositories
{
    public class ReserveringRepository : IReserveringRepository
    {
        // Pas deze connection string aan naar jouw XAMPP-config
        private const string ConnectionString =
            "Server=localhost;Port=3306;Database=campingapp;User ID=root;Password=;";

        public void Add(Reservering reservering)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO reservering
                    (startdatum, einddatum, veld_id, staanplaats_id, accommodatie_id)
                VALUES
                    (@start, @end, @veldId, @staanplaatsId, @accommodatieId);";

            command.Parameters.Add("@start", MySqlDbType.Date).Value = reservering.StartDatum;
            command.Parameters.Add("@end", MySqlDbType.Date).Value = reservering.EindDatum;
            command.Parameters.Add("@veldId", MySqlDbType.Int32).Value = reservering.VeldId;
            command.Parameters.Add("@staanplaatsId", MySqlDbType.Int32).Value = reservering.StaanplaatsId;
            command.Parameters.Add("@accommodatieId", MySqlDbType.Int32).Value = reservering.AccommodatieId;

            command.ExecuteNonQuery();

            // In MySqlConnector is LastInsertedId ook beschikbaar
            reservering.Id = (int)command.LastInsertedId;
        }
    }
}
