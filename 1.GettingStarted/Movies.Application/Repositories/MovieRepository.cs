using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Movies.Application.Repositories;

internal class MovieRepository : IMovieRepository
{   
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public MovieRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);      
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
             INSERT INTO Movies (Id, title, slug, yearOfRelease)
            VALUES (@Id, @Title, @Slug, @YearOfRelease);
            """, movie, transaction, cancellationToken:token));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.QueryFirstOrDefaultAsync<int>("""
                    INSERT INTO Genres (movieId, name) 
                    values (@MovieId, @Name)
                    """,
                    new { MovieId = movie.Id, Name = genre } , transaction:transaction
                   );
            }
        }
        transaction.Commit();
      
        return result > 0;

    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
            select * from movies where id = @id
            """, new { id }, cancellationToken: token));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
            select name from genres where movieid = @id 
            """, new { id }, cancellationToken: token));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }
    public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
            select * from movies where slug = @slug
            """, new { slug }, cancellationToken: token));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
            select name from genres where movieid = @id 
            """, new { id = movie.Id }, cancellationToken: token));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;

    }
    public async Task<IEnumerable<Movie>> GetAllAsync( CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        string sql = @"
            SELECT m.*, STRING_AGG(g.name, ',') as genres 
FROM movies m left join genres g on m.id = g.movieid
group by id ,title,slug,yearOfRelease
            ";


        var result = await connection.QueryAsync(sql, token);

        return result.Select(x => new Movie
        {
            Id = x.Id,
            Title = x.title,
            YearOfRelease = x.yearofrelease,
            Genres = Enumerable.ToList(x.genres.Split(','))
        });

    }

    public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            DELETE from genres where movieid = @id
            """, new { id = movie.Id }, cancellationToken: token));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                    INSERT into genres (movieId, name) 
                    values (@MovieId, @Name)
                    """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            UPDATE movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease 
            where id = @Id
            """, movie, cancellationToken: token));

        transaction.Commit();
        return result > 0;

    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            DELETE from genres where movieid = @id
            """, new { id }, cancellationToken: token));

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            DELETE from movies where id = @id
            """, new { id }, cancellationToken: token));

        transaction.Commit();
        return result > 0;

    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var result =  await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition("""
            SELECT * from movies where id = @id
            """, new { id }, cancellationToken: token));

        return result is not null;

    }


}
