﻿namespace Movies.Api.Endpoints.Movies;

public static class MovieEndpointsExtension
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetMovie();
        app.MapCreateMovie();
        app.MapGetAllMovies();
        app.MapUpdateMovie();
        app.MapDeleteMovie();
        
        return app;
    }
}
