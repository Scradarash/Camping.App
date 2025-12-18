using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using MySqlConnector;

namespace Camping.Core.Data.Repositories
{
    public class ReserveringRepository : IReserveringRepository
    {
        private readonly MySqlDbExecutor _db;

        public ReserveringRepository(MySqlDbExecutor db)
        {
            _db = db;
        }

        public async Task AddAsync(Reservering reservering, int reserveringhouderId)
        {
            const string sql = @"
                INSERT INTO reserveringen
                    (aankomstdatum, vertrekdatum, staanplaats_id, accommodatie_type_id, reserveringhouder_id, totaal_prijs, is_betaald)
                VALUES
                    (@start, @end, @staanplaatsId, @accommodatieId, @houderId, @totaal, 0);
                SELECT LAST_INSERT_ID();";

            long id = await _db.ExecuteScalarAsync<long>(sql, cmd =>
            {
                cmd.Parameters.Add("@start", MySqlDbType.Date).Value = reservering.StartDatum.Date;
                cmd.Parameters.Add("@end", MySqlDbType.Date).Value = reservering.EindDatum.Date;
                cmd.Parameters.Add("@staanplaatsId", MySqlDbType.Int32).Value = reservering.StaanplaatsId;
                cmd.Parameters.Add("@accommodatieId", MySqlDbType.Int32).Value = reservering.AccommodatieId;
                cmd.Parameters.Add("@houderId", MySqlDbType.Int32).Value = reserveringhouderId;
                cmd.Parameters.Add("@totaal", MySqlDbType.Decimal).Value = reservering.TotaalPrijs;
            });

            reservering.Id = (int)id;
        }
    }
}