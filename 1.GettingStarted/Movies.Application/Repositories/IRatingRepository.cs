﻿using Movies.Application.Models;

namespace Movies.Application.Repositories;

public interface IRatingRepository
{
    Task<bool> RateMovie(Guid movieId, int rating , Guid userId ,CancellationToken token=default);

    Task<float?> GetRatingAsync(Guid movieId,CancellationToken token=default);

    Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid movieId , Guid userId, CancellationToken token = default);

    Task<bool> DeleteRatingAsync(Guid movieId,Guid userId, CancellationToken token = default);

    Task<IEnumerable<MovieRating>> GetRatingForUserAsync(Guid userId,CancellationToken token=default);
}
