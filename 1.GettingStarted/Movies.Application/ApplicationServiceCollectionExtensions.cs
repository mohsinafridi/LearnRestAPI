using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Repositories;

namespace Movies.Application;

public static class ApplicationServiceCollectionExtensions 
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register repositories
        services.AddSingleton<IMovieRepository, MovieRepository>();
      
        return services;
    }
}
