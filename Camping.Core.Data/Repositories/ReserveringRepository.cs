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
            const string reserveringSql = @"
        INSERT INTO reserveringen
            (aankomstdatum, vertrekdatum, staanplaats_id, accommodatie_type_id, reserveringhouder_id, totaal_prijs, is_betaald)
        VALUES
            (@start, @end, @staanplaatsId, @accommodatieId, @houderId, @totaal, 0);
        SELECT LAST_INSERT_ID();";

            int reserveringId = await _db.ExecuteScalarAsync<int>(reserveringSql, cmd =>
            {
                cmd.Parameters.Add("@start", MySqlDbType.Date).Value = reservering.StartDatum.Date;
                cmd.Parameters.Add("@end", MySqlDbType.Date).Value = reservering.EindDatum.Date;
                cmd.Parameters.Add("@staanplaatsId", MySqlDbType.Int32).Value = reservering.StaanplaatsId;
                cmd.Parameters.Add("@accommodatieId", MySqlDbType.Int32).Value = reservering.AccommodatieId;
                cmd.Parameters.Add("@houderId", MySqlDbType.Int32).Value = reserveringhouderId;
                cmd.Parameters.Add("@totaal", MySqlDbType.Decimal).Value = reservering.TotaalPrijs;
            });

            reservering.Id = reserveringId;

            // 2️⃣ Gasten opslaan
            foreach (var gast in reservering.Gastenlijst)
            {
                await AddGastAanReservering(gast, reserveringId);
            }
        }

        private async Task AddGastAanReservering(Gast gast, int reserveringId)
        {
            const string gastSql = @"
        INSERT INTO gasten (naam, geboortedatum)
        VALUES (@naam, @geboortedatum);
        SELECT LAST_INSERT_ID();";

            int gastId = await _db.ExecuteScalarAsync<int>(gastSql, cmd =>
            {
                cmd.Parameters.Add("@naam", MySqlDbType.VarChar).Value = gast.Naam;
                cmd.Parameters.Add("@geboortedatum", MySqlDbType.Date)
                    .Value = gast.Geboortedatum.ToDateTime(TimeOnly.MinValue);
            });

            const string koppelSql = @"
        INSERT INTO reservering_gasten (gast_id, reservering_id)
        VALUES (@gastId, @reserveringId);";

            await _db.ExecuteScalarAsync<int>(koppelSql, cmd =>
            {
                cmd.Parameters.Add("@gastId", MySqlDbType.Int32).Value = gastId;
                cmd.Parameters.Add("@reserveringId", MySqlDbType.Int32).Value = reserveringId;
            });
        }


    }
}