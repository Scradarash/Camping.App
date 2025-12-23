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

        public int Execute(
            string sql,
            Action<MySqlCommand>? parameterize)
        {
            using var connection = _db.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            parameterize?.Invoke(command);

            return command.ExecuteNonQuery();
        }

        public T ExecuteScalar<T>(
            string sql,
            Action<MySqlCommand>? parameterize)
        {
            using var connection = _db.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            parameterize?.Invoke(command);

            object? value = command.ExecuteScalar();
            if (value is null || value is DBNull)
                return default!;

            return (T)Convert.ChangeType(value, typeof(T));
        }

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

        public async Task<int> ExecuteAsync(
            string sql,
            Action<MySqlCommand>? parameterize)
        {
            await using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            parameterize?.Invoke(command);

            return await command.ExecuteNonQueryAsync();
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
