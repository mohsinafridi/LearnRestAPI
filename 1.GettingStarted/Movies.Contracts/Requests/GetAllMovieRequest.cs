namespace Movies.Contracts.Requests;

public class GetAllMovieRequest : PagedRequest
{
    public required string? Title { get; init; }
    public required int? YearOfRelease{ get; init; }
    public required string? SortBy { get; init; }
}
