using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using MySqlConnector;

namespace Camping.Core.Data.Repositories;

public class VeldRepository : IVeldRepository
{
    private readonly MySqlDbExecutor _db;

    public VeldRepository(MySqlDbExecutor db)
    {
        _db = db;
    }

    public IEnumerable<Veld> GetAll()
    {
        const string sql = @"SELECT id, naam, x_position, y_position, width, height, image_name, description FROM velden;";
        return _db.Query(sql, null, Map);
    }

    private static Veld Map(MySqlDataReader reader)
    {
        return new Veld
        {
            id = reader.GetInt32("id"),
            Name = reader.GetString("naam"),
            XPosition = reader.GetFloat("x_position"),
            YPosition = reader.GetFloat("y_position"),
            Width = reader.GetFloat("width"),
            Height = reader.GetFloat("height"),
            ImageName = reader.GetString("image_name"),
            Description = reader.GetString("description")
        };
    }
}