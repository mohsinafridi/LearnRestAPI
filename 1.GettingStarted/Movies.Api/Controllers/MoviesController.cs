using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;


[ApiController]
[ApiVersion(1.0)]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IOutputCacheStore _outputCacheStore;

    public MoviesController(IMovieService movieService, IOutputCacheStore outputCacheStore)
    {
        _movieService = movieService;
        _outputCacheStore = outputCacheStore;
    }

    [HttpPost]
    [Route(ApiEndpoints.Movies.Create)]
    //  [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request , CancellationToken token)
    {
        var movie = request.MapToMovie();
        var result = await _movieService.CreateAsync(movie, token);
        await _outputCacheStore.EvictByTagAsync("movies", token);

        var response = movie.MapToResponse();

        return CreatedAtAction(nameof(Get),new { idOrSlug = movie.Id }, response);
    }

     
    [HttpGet(ApiEndpoints.Movies.Get)]
    [OutputCache(PolicyName = "MoviesCache")]
    // [ResponseCache(Duration = 30 , Location = ResponseCacheLocation.Any , VaryByHeader = "Accept , Accept-Encoding")]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var movie = Guid.TryParse(idOrSlug, out var id)
           ? await _movieService.GetByIdAsync(id, userId, token)
           : await _movieService.GetBySlugAsync(idOrSlug, userId, token);
        if (movie is null)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);
    }

   

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    [OutputCache(PolicyName = "MoviesCache")]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]    
    public async Task<IActionResult> GetAll([FromQuery]GetAllMovieRequest request,CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var options = request.MapToOption().WithUser(userId);

        var movies = await _movieService.GetAllAsync(options, token);

        var moviesCount = await _movieService.GetCountAsync(options.Title,options.YearOfRelease, token);

        var moviesResponse = movies.MapToResponse(request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
            request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
            moviesCount);

        return Ok(moviesResponse);
    }


    [HttpPut(ApiEndpoints.Movies.Update)]
    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movie =  request.MapToMovie(id);

      var updatedMovie =  await _movieService.UpdateAsync(movie, userId, token);
        if (updatedMovie is null)
        {
            return NotFound();
        }
        await _outputCacheStore.EvictByTagAsync("movies", token);
        var response = updatedMovie.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]    
    [Authorize(AuthConstants.AdminUserPolicyName)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        // var userId = HttpContext.GetUserId();
        var deleted = await _movieService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }
        await _outputCacheStore.EvictByTagAsync("movies", token);
        return Ok();
    }
}