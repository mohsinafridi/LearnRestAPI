namespace Movies.Application.Repositories;

public interface IMovieRepository
{
    Task<bool> CreateAsync(Models.Movie movie , CancellationToken token = default);
    Task<Models.Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default);
    Task<Models.Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default);    
    Task<IEnumerable<Models.Movie>> GetAllAsync(Guid? userId = default, CancellationToken token = default);
    
    Task<bool> UpdateAsync(Models.Movie movie, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
}
