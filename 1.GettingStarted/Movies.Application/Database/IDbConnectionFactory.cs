namespace Movies.Application.Database;

public interface IDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory()
    {
        _connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
    }
}