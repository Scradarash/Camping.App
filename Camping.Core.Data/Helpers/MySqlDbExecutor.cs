using MySqlConnector;

namespace Camping.Core.Data.Helpers
{
    public sealed class MySqlDbExecutor
    {
        private readonly DbConnection _db;

        public MySqlDbExecutor(DbConnection db)
        {
            _db = db;
        }

        //Deze methode returnt alle rijen van een tabel, bijvoorbeeld voorzieningen, staanplaatsen, velden of accommodaties
        public IEnumerable<T> Query<T>(
            string sql,
            Action<MySqlCommand>? parameterize,
            Func<MySqlDataReader, T> map)
        {
            var result = new List<T>();

            using var connection = _db.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            parameterize?.Invoke(command);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(map(reader));
            }

            return result;
        }

        //Deze methode returnt alleen één of 0 rijen. Dit is bijvoorbeeld handig voor
        //GetByEmail of GetByNaam, waar de code alleen één rij verwacht, dus het is onnodig om elke keer alle rijen in te lezen

        public T? QuerySingleOrDefault<T>(
            string sql,
            Action<MySqlCommand>? parameterize,
            Func<MySqlDataReader, T> map)
            where T : class
        {
            using var connection = _db.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            parameterize?.Invoke(command);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return map(reader);
        }
        
        //Zelfde methode als Query hierboven, maar met Async. Dit wordt gebruikt bij UI gerelateerd DB interacties
        //Hierdoor hoeft de UI niet te wachten en kan het zonder stotteren zich aanpassen
        public async Task<List<T>> QueryAsync<T>(
            string sql,
            Action<MySqlCommand>? parameterize,
            Func<MySqlDataReader, T> map)
        {
            var result = new List<T>();

            await using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            parameterize?.Invoke(command);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(map(reader));
            }

            return result;
        }

        //Zelfde methode als QuerySingleOrDefault hierboven, maar met Async. Dit wordt gebruikt bij UI gerelateerd DB interacties
        //Hierdoor hoeft de UI niet te wachten en kan het zonder stotteren zich aanpassen
        public async Task<T?> QuerySingleOrDefaultAsync<T>(
            string sql,
            Action<MySqlCommand>? parameterize,
            Func<MySqlDataReader, T> map)
            where T : class
        {
            await using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            parameterize?.Invoke(command);

            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return null;
            }

            return map(reader);
        }

       
        public async Task<T> ExecuteScalarAsync<T>(
            string sql,
            Action<MySqlCommand>? parameterize)
        {
            await using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            parameterize?.Invoke(command);

            object? value = await command.ExecuteScalarAsync();
            if (value is null || value is DBNull)
                return default!;

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
