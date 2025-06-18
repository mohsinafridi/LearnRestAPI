using Azure.Core;
using FluentValidation;
using Movies.Application.Models;

namespace Movies.Contracts.Requests;

public class GetAllMoviesOptionsValidator :AbstractValidator<GetAllMoviesOptions>
{
    private static readonly string[] AcceptableSortFields = new[]
    {
        "title", "yearofrelease",       
    };
    public GetAllMoviesOptionsValidator()
    {
        RuleFor(x => x.YearOfRelease)
        .LessThanOrEqualTo(DateTime.Now.Year);

        RuleFor(x=>x.SortField)
            .Must(x=>x is null || AcceptableSortFields.Contains(x,StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Sort field must be one of the following: " +
            $"{string.Join(", ", AcceptableSortFields)}");

        RuleFor(x => x.Page)
               .GreaterThanOrEqualTo(1)
               .WithMessage("Page must be greater than or equal to 1");

        RuleFor(x=>x.PageSize)
            .InclusiveBetween(1,25)
            .WithMessage("Page size must be between 1 and 25");
    }
}
