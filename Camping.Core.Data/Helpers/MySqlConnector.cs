using Camping.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace Camping.Core.Data.Helpers
{
    public class DbConnection
    {
        private readonly string _connectionString;

        // Connectie gegevens invoeren database in constructor
        // Gegevens veranderen naar je eigen gegevens (vooral UserID, Port en Password)
        public DbConnection()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "camping_demo",
                Password = "",
                Database = "campingapp",
                Port = 3306
            };

            _connectionString = builder.ConnectionString;
        }

        // Methode geeft SQL connectie terug
        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

    }
}
