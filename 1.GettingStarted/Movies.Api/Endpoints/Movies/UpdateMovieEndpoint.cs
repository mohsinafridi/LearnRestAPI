
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {

        app.MapPut(ApiEndpoints.Movies.Update, async (Guid id, UpdateMovieRequest request, IMovieService movieService, IOutputCacheStore outputCacheStore,
            HttpContext context, CancellationToken token) =>
       {
           var userId = context.GetUserId();

           var movie = request.MapToMovie(id);


           var updatedMovie = await movieService.UpdateAsync(movie, userId, token);
           if (updatedMovie is null)
           {
               return Results.NotFound();
           }
           await outputCacheStore.EvictByTagAsync("movies", token);
           var response = updatedMovie.MapToResponse();
           return TypedResults.Ok(response);
       }).
       WithName(Name)
        .RequireAuthorization(AuthConstants.TrustedMemberPolicyName)
       .WithSummary("Update Movie")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

        return app;

    }
}
