using System.Data;
using Microsoft.Data.SqlClient;
using Npgsql;


namespace Movies.Application.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}

public class NpgsqlConnectionFactor : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactor(string connectionString)
    {
        _connectionString = connectionString;
    }
   

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}

public class SqlServerConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public SqlServerConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}