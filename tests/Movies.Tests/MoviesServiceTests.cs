using Movies.Application.Repositories;
using Movies.Application.Services;
using Moq;
using FluentValidation;
using Movies.Application.Models;


namespace Movies.Tests;

public class MoviesServiceTests
{
    private readonly MovieService _movieService;
    private readonly IValidator<Movie> _moviesValidatorMock;
    private readonly IValidator<GetAllMoviesOptions> _getAllMoviesOptionsRepositoryMock;

    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IRatingRepository> _ratingRepositoryMock;

    public MoviesServiceTests()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _ratingRepositoryMock = new Mock<IRatingRepository>();
        _moviesValidatorMock = new Mock<IValidator<Movie>>().Object; // Fix: Initialize _moviesValidatorMock to avoid null reference.  
        _getAllMoviesOptionsRepositoryMock = new Mock<IValidator<GetAllMoviesOptions>>().Object; // Fix: Initialize _getAllMoviesOptionsRepositoryMock to avoid null reference.  

        _movieService = new MovieService(_ratingRepositoryMock.Object,
            _movieRepositoryMock.Object,
            _moviesValidatorMock,
            _getAllMoviesOptionsRepositoryMock
        );
    }

    [Fact]
    public async void GetMovies_ReturnAll()
    {
        // Arrange  
        var movies = new List<Movie>()
       {
           new Movie
           {
               Id = Guid.NewGuid(),
               Title = "Movie 1",
               Genres = new List<string> { "Action", "Adventure" },
               YearOfRelease = 2020,
           },
           new Movie
           {
               Id = Guid.NewGuid(),
               Title = "Movie 2",
               Genres = new List<string> { "Action", "Adventure" },
               YearOfRelease = 2021,
           }
       };

        _movieRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<GetAllMoviesOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(movies);

        // Act  
        var result = await _movieService.GetAllAsync(new GetAllMoviesOptions
        {
            Title = null,
            YearOfRelease = null,
            Page = 1,
            PageSize = 10
        }, CancellationToken.None);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}
