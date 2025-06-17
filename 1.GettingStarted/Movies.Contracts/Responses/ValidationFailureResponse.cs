
namespace Movies.Contracts.Responses;

public class ValidationFailureResponse
{
    public required IEnumerable<ValidatioResponse> Errors { get; init; }
}

public class ValidatioResponse
{

    public required string  PropertyName { get; init; }
    public required string Message { get; init; }
}
