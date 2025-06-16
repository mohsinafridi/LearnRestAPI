
using Dapper;

namespace Movies.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbconnectionFactory;
    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbconnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbconnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
            create table if not exists movies(
            id UUID primary key,
            slug TEXT not null,
            title TEXT not null,
            yearOfrelease integer not null);
            """);

        await connection.ExecuteAsync("""
            create unique index concurrently if not exist movies_slug_index
            on movies
            using btree(slug);
            """);
    }

}
