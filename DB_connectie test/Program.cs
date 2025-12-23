using Camping.Core.Data.Helpers;
using Camping.Core.Data.Repositories;
using Camping.Core.Interfaces.Repositories;

namespace DB_connectie_test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dbConnection = new DbConnection();

            var db = new MySqlDbExecutor(dbConnection);

            IGastRepository repo = new MySqlGastRepository(db);

            var gasten = await repo.GetAllAsync();
            foreach (var gast in gasten)
            {
                Console.WriteLine($"{gast.Id}, {gast.Naam}");
            }

            await using var conn = dbConnection.CreateConnection();
            try
            {
                await conn.OpenAsync();
                Console.WriteLine("Database connectie succesvol!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database connectie mislukt!");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
