namespace Movies.Application.Models;

public class GetAllMoviesOptions
{
    public required string? Title { get; init; }
    public required int? YearOfRelease { get; init; }
    public Guid? UserId { get; set; }
    public string? SortField { get; init; }
    public SortOrder SortOrder { get; init; }
    public required int? Page { get; set; } = 1;
    public required int? PageSize { get; set; } = 10;
}

public enum SortOrder
{
    Unsorted,
    Ascending,
    Descending
}
