namespace Movies.Application.Repositories;

public interface IMovieRepository
{
    Task<bool> CreateAsync(Models.Movie movie);
    Task<Models.Movie?> GetByIdAsync(Guid id);
    Task<IEnumerable<Models.Movie>> GetAllAsync();
    
    Task<bool> UpdateAsync(Models.Movie movie);
    
    Task<bool> DeleteByIdAsync(Guid id);
}
