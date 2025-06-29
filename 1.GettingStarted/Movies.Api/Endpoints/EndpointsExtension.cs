﻿using Movies.Api.Endpoints.Movies;
using Movies.Api.Endpoints.Ratings;

namespace Movies.Api.Endpoints;

public static class EndpointsExtension 
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapMovieEndpoints();
        app.MapRatingEndpoints();
        return app;
    }
}
