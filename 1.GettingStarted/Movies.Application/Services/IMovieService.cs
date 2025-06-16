using Movies.Application.Models;

namespace Movies.Application.Services;

public interface IMovieService
{
    Task<bool> CreateAsync(Models.Movie movie , CancellationToken token = default);
    Task<Models.Movie?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<Models.Movie?> GetBySlugAsync(string slug, CancellationToken token = default);
    Task<IEnumerable<Models.Movie>> GetAllAsync(CancellationToken token = default);

    Task<Movie> UpdateAsync(Models.Movie movie, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}
