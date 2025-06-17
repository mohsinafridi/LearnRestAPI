using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;
using Movies.Application.Services;

namespace Movies.Application;

public static class ApplicationServiceCollectionExtensions 
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register repositories
        services.AddSingleton<IMovieRepository, MovieRepository>();
        services.AddSingleton<IMovieService, MovieService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarket>(ServiceLifetime.Singleton);

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,string connectionString)
    {
       // services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactor(connectionString));

        services.AddSingleton<IDbConnectionFactory>(_ => new SqlServerConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}
