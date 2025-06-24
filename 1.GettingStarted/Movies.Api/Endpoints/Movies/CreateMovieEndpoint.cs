using Azure.Core;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;


public static class CreateMovieEndpoint
{
    public const string Name = "CreateMovie";

    public static IEndpointRouteBuilder MapCreateMovie(this IEndpointRouteBuilder app)
    {

        app.MapPost(ApiEndpoints.Movies.Create, async (CreateMovieRequest request, IMovieService movieService, IOutputCacheStore outputCacheStore , CancellationToken token) =>
        {
            var movie = request.MapToMovie();

            var result = await movieService.CreateAsync(movie, token);
            
            await outputCacheStore.EvictByTagAsync("movies", token);

            var response = movie.MapToResponse();

            return TypedResults.CreatedAtRoute(response, GetMovieEndpoint.Name, new { idOrSlug = movie.Id });
        }).WithName(Name)
        .RequireAuthorization(AuthConstants.TrustedMemberPolicyName)
            .WithDescription("Create a new movie")
            .Produces<MovieResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        return app; 
    
    } 
}