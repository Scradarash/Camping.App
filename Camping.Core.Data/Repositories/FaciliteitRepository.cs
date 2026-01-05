using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using MySqlConnector;

namespace Camping.Core.Data.Repositories
{
    public class FaciliteitRepository : IFaciliteitRepository
    {
        private readonly MySqlDbExecutor _db;

        public FaciliteitRepository(MySqlDbExecutor db)
        {
            _db = db;
        }

        public IEnumerable<Faciliteit> GetFaciliteiten()
        {
            const string sql = @"SELECT id, naam, omschrijving, image_name, x_position, y_position, width, height FROM faciliteiten;";
            return _db.Query(sql, null, Map);
        }

        private static Faciliteit Map(MySqlDataReader reader)
        {
            return new Faciliteit
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("naam"),
                Description = reader.IsDBNull(reader.GetOrdinal("omschrijving")) ? string.Empty : reader.GetString("omschrijving"),
                ImageName = reader.GetString("image_name"),
                XPosition = reader.GetFloat("x_position"),
                YPosition = reader.GetFloat("y_position"),
                Width = reader.GetFloat("width"),
                Height = reader.GetFloat("height")
            };
        }
    }
}
