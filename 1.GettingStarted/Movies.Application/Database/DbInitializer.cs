
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            create unique index concurrently if not exists movies_slug_idx
            on movies
            using btree(slug);
            """);

        await connection.ExecuteAsync("""                
        create table if not exists genres (
            movieId UUID  references movies (Id),
            name TEXT not null);        
        """);
    }

    public async Task InitalizeSqlServerAsync()
    {
        using var connection = await _dbconnectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Movies' AND type = 'U')
            BEGIN
            CREATE TABLE Movies(
            Id uniqueidentifier  PRIMARY KEY,
            Title NVARCHAR(255) NOT NULL,
            Slug NVARCHAR(255) UNIQUE NOT NULL,
            YearOfRelease INT NULL);
             END
            """);

        await connection.ExecuteAsync("""            
            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Movies_Slug'
             AND object_id = OBJECT_ID('Movies'))
            BEGIN
            CREATE NONCLUSTERED INDEX IX_Movies_Slug
            ON Movies (slug);
            END
            """);

        await connection.ExecuteAsync("""
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Genres' AND type = 'U')
        BEGIN  
        CREATE table Genres (
            MovieId uniqueidentifier  references Movies (Id),
            Name NVARCHAR(255) not null);
         END
        """);
    }   
}
 
