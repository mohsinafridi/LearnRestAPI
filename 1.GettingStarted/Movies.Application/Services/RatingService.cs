using FluentValidation.Results;
using Movies.Application.Models;
using Movies.Application.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Movies.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IMovieRepository _movieRepository;

    public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
    {
        _ratingRepository = ratingRepository;
        _movieRepository = movieRepository;
    }
    public Task<float?> GetRatingAsync(Guid movieId, CancellationToken token = default)
    {

        throw new NotImplementedException();
    }

    public Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RateMovieAsync(Guid movieId, Guid userId, int rating, CancellationToken token = default)
    {
        if (rating is <= 0 or > 5)
        {
            var validationFailures = new List<ValidationFailure>
           {
               new ValidationFailure
               {
                   PropertyName = "Rating",
                   ErrorMessage = "Rating must be between 1 and 5"
               }
           };

            throw new ValidationException("Validation failed", null);
        }
       
        var movieExists = await _movieRepository.ExistsByIdAsync(movieId, token);
        if (!movieExists)
        {
            return false;
        }
        return await _ratingRepository.RateMovie(movieId, rating, userId, token);

    }

    public Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
    {
        return _ratingRepository.DeleteRatingAsync(movieId, userId, token);
    }

    public Task<IEnumerable<MovieRating>> GetRatingForUserAsync(Guid userId, CancellationToken token = default)
    {
        return _ratingRepository.GetRatingForUserAsync(userId, token);
    }
}
