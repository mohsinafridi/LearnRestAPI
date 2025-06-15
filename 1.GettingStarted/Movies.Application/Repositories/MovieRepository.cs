
using Movies.Application.Models;

namespace Movies.Application.Repositories;

internal class MovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();
    public  Task<bool> CreateAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        var movie = _movies.SingleOrDefault(m => m.Id == id);
        return Task.FromResult(movie);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
       var moviesIndex = _movies.FindIndex(m => m.Id == movie.Id);
        if(moviesIndex < 0)
        {
            return Task.FromResult(false);
        }
        _movies[moviesIndex] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removedCount = _movies.RemoveAll(m => m.Id == id);
        var movieRemoved = removedCount > 0;
        return Task.FromResult(movieRemoved);
    }
}
