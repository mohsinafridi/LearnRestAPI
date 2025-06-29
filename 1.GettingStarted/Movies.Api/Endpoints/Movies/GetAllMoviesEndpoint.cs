﻿
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;

public static class GetAllMoviesEndpoint
{
    public const string Name = "GetAllMovies";

    public static IEndpointRouteBuilder MapGetAllMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Movies.GetAll, async ([AsParameters] GetAllMovieRequest request, IMovieService movieService, HttpContext context, CancellationToken token) =>
        {
            var userId = context.GetUserId();

            var options = request.MapToOption().WithUser(userId);

            var movies = await movieService.GetAllAsync(options, token);

            var moviesCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);

            var moviesResponse = movies.MapToResponse(request.Page.GetValueOrDefault(PagedRequest.DefaultPageSize),
                request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
                moviesCount);

            return TypedResults.Ok(moviesResponse);
        })
        .WithName($"{Name}V1")
        .WithDescription("Get All Movies")
        .Produces<MoviesResponse>(StatusCodes.Status200OK)
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0).CacheOutput("MoviesCache");


        app.MapGet(ApiEndpoints.Movies.GetAll, async ([AsParameters] GetAllMovieRequest request, IMovieService movieService, HttpContext context, CancellationToken token) =>
        {
            var userId = context.GetUserId();

            var options = request.MapToOption().WithUser(userId);

            var movies = await movieService.GetAllAsync(options, token);

            var moviesCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);

            var moviesResponse = movies.MapToResponse(request.Page.GetValueOrDefault(PagedRequest.DefaultPageSize),
                request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
                moviesCount);

            return TypedResults.Ok(moviesResponse);
        })
        .WithName($"{Name}V2")
        .WithDescription("Get All Movies")
        .Produces<MoviesResponse>(StatusCodes.Status200OK)
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(2.0)
        .CacheOutput("MoviesCache");

        return app;
    }
}
