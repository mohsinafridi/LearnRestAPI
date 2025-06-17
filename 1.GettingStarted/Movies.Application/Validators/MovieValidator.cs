using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMovieRepository _movieRepository;

    public MovieValidator(IMovieRepository movieRepository)
    {

        RuleFor(m => m.Id)
                       .NotEmpty().WithMessage("Id is required.");


        RuleFor(movie => movie.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(m => m.YearOfRelease).NotEmpty()
            .WithMessage("Year of Release is required.")
            .LessThanOrEqualTo(DateTime.Now.Year);
        
        RuleFor(m=>m.Genres)
            .NotEmpty().WithMessage("At least one genre is required.")
            .Must(genres => genres.Count <= 5).WithMessage("A maximum of 5 genres is allowed.")
            .ForEach(genre => genre.NotEmpty().WithMessage("Genre cannot be empty."))
            .ForEach(genre => genre.MaximumLength(50).WithMessage("Genre must not exceed 50 characters."));

        
    }

    private async Task<bool> ValidateSlug(Movie movie,string slug, CancellationToken token)
    { 
    var existingMovie  = await _movieRepository.GetBySlugAsync(slug, token);
        if (existingMovie is not null)
        {
            return existingMovie.Id == movie.Id;
        }
        return existingMovie is null;
    }
}
