using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;


namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IMovieRepository _movieRepository;
   
    private readonly IValidator<Movie> _movieValidator;
    private readonly IValidator<GetAllMoviesOptions> _optionsValidator;
    public MovieService(IRatingRepository ratingRepository, IMovieRepository movieRepository, 
        IValidator<Movie> movieValidator,
        IValidator<GetAllMoviesOptions> optionsValidator)
    {
        _ratingRepository = ratingRepository;
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
        _optionsValidator = optionsValidator;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie , token);
        return await _movieRepository.CreateAsync(movie ,token);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _movieRepository.DeleteByIdAsync(id, token);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options ,CancellationToken token = default)
    {
        await _optionsValidator.ValidateAndThrowAsync(options , token);

        return await _movieRepository.GetAllAsync(options, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default ,CancellationToken token = default)
    {
        return _movieRepository.GetByIdAsync(id, userId, token);
    }

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default , CancellationToken token = default)
    {
        return _movieRepository.GetBySlugAsync(slug, userId,token);
    }
    public async  Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default , CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie , token);
        var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id, token);
        if (!movieExists)
        {
            return null;
        }

        await  _movieRepository.UpdateAsync(movie, token);

        if (!userId.HasValue)
        {
            movie.Rating = await _ratingRepository.GetRatingAsync(movie.Id, token);
        }

        var rating = await _ratingRepository.GetRatingAsync(movie.Id, userId.Value, token);
        movie.Rating = rating.Rating;
        movie.UserRating = rating.UserRating;
        return movie;

    }

    public async Task<int> GetCountAsync(string? title, int? yearofRelease, CancellationToken token = default)
    {
        return await _movieRepository.GetCountAsync(title,yearofRelease,token);
    }
}
