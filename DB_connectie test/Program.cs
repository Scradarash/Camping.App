using Camping.Core.Data.Helpers;
using Camping.Core.Data.Repositories;
using Camping.Core.Interfaces.Repositories;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace DB_connectie_test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Maak een DbConnection object
            var db = new DbConnection();

            // Injecteer DB repo
            IGastRepository repo = new MySqlGastRepository(db);

            var gasten = await repo.GetAllAsync();

            foreach (var gast in gasten)
            {
                Console.WriteLine($"{gast.Id}, {gast.Naam}");
            }

            // Maak een connection
            await using var conn = db.CreateConnection();

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
