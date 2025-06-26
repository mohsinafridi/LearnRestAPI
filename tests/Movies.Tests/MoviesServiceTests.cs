using Movies.Application.Repositories;
using Movies.Application.Services;
using Moq;
using FluentValidation;
using Movies.Application.Models;


namespace Movies.Tests;

public class MoviesServiceTests
{

    private readonly MovieService _movieService;
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IRatingRepository> _ratingRepositoryMock;


    public MoviesServiceTests()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _movieService = new MovieService(          
            _ratingRepositoryMock.Object,
            _movieRepositoryMock.Object,
            new Mock<IValidator<Movie>>().Object,
            new Mock<IValidator<GetAllMoviesOptions>>().Object
        );
    }
    [Fact]
    public void GetMovies_ReturnAll()
    {


        // Arrane
               

       // Act
    


        // Assert

      
        

    }
}