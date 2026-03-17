using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Database
{
    public interface ISchemaManager
    {
        Task EnsureSchemaExistsAsync(string schemaName);
        Task EnsureSchemasExistAsync(IEnumerable<string> schemaNames);
        Task<string> GetConnectionStringWithSchema(string schemaName);
    }

    public class PostgresSchemaManager : ISchemaManager
    {
        private readonly string _connectionString;

        public PostgresSchemaManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task EnsureSchemaExistsAsync(string schemaName)
        {
            await EnsureSchemasExistAsync(new[] { schemaName });
        }

        public async Task EnsureSchemasExistAsync(IEnumerable<string> schemaNames)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            foreach (var schemaName in schemaNames)
            {
                var sql = $"CREATE SCHEMA IF NOT EXISTS \"{schemaName}\"";
                await using var command = new Npgsql.NpgsqlCommand(sql, connection);
                await command.ExecuteNonQueryAsync();
            }
        }


        public Task<string> GetConnectionStringWithSchema(string schemaName)
        {
            var builder = new Npgsql.NpgsqlConnectionStringBuilder(_connectionString);
            builder.SearchPath = schemaName;
            return Task.FromResult(builder.ToString());

        }
    }
}
