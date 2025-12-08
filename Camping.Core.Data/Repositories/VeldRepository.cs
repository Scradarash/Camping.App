using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using System.Collections.Generic;

namespace Camping.Core.Data.Repositories;

public class VeldRepository : IVeldRepository
{
    private readonly DbConnection _db;

    public VeldRepository(DbConnection db)
    {
        _db = db;
    }

    public IEnumerable<Veld> GetAll()
    {
        var velden = new List<Veld>();

        using var connection = _db.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM velden";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            velden.Add(new Veld
            {
                id = reader.GetInt32("id"),
                Name = reader.GetString("naam"),

                // Kreeg een error dat een double niet t zelfde is als float, dus op deze vieze manier opgelost.
                XPosition = reader.GetFloat("x_position"),
                YPosition = reader.GetFloat("y_position"),
                Width = reader.GetFloat("width"),
                Height = reader.GetFloat("height"),
                ImageName = reader.GetString("image_name"),
                Description = reader.GetString("description")
            });
        }

        return velden;
    }
}