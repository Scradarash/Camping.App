using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;
using MySqlConnector;

namespace Camping.Core.Data.Repositories
{
    public class FaciliteitRepository : IFaciliteitRepository
    {
        private readonly DbConnection _dbConnection;

        public FaciliteitRepository(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Query om alle faciliteiten op te halen
        public IEnumerable<Faciliteit> GetFaciliteiten()
        {
            try
            {
                using MySqlConnection connection = _dbConnection.CreateConnection();
                connection.Open();

                string query = "SELECT * FROM faciliteiten";

                using MySqlCommand command = new MySqlCommand(query, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                return MapToFaciliteiten(reader);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching faciliteiten: {ex.Message}");
                return new List<Faciliteit>();
            }
        }

        // Functie om ze te mappen van database rijen naar Faciliteit objecten
        private List<Faciliteit> MapToFaciliteiten(MySqlDataReader reader)
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();

            while (reader.Read())
            {
                Faciliteit faciliteit = new Faciliteit
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("naam"),
                    // Als er geen omschrijving is, zet dan het woord "omschrijving" als de omschrijving, zal crashes voorkomen als er null is
                    Description = reader.IsDBNull(reader.GetOrdinal("omschrijving"))
                        ? string.Empty
                        : reader.GetString("omschrijving"),

                    ImageName = reader.GetString("image_name"),
                    XPosition = reader.GetFloat("x_position"),
                    YPosition = reader.GetFloat("y_position"),
                    Width = reader.GetFloat("width"),
                    Height = reader.GetFloat("height")
                };

                faciliteiten.Add(faciliteit);
            }

            return faciliteiten;
        }
    }
}