namespace Movies.Api.Endpoints.Ratings;

public static class RatingEndpointExtension
{
    public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRateMovie();
        app.MapGetUserRatings();
        app.MapDeleteRating();

        return app;
    }
}
