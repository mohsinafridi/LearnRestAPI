using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    [Route(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request , CancellationToken token)
    {
        var movie = request.MapToMovie();
        var result = await _movieService.CreateAsync(movie, token);
        return CreatedAtAction(nameof(Get),new { id = movie.Id }, movie);
    }


    [HttpGet]
    [Route(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
    {
        var movie = await _movieService.GetByIdAsync(id, token);
        if (movie == null)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpGet]
    [Route(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var movies = await _movieService.GetAllAsync(token);

        var moviesResponse = movies.MapToResponse();

        return Ok(moviesResponse);
    }


    [HttpPut]
    [Route(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token)
    {
        var movie =  request.MapToMovie(id);

      var updatedMovie =  await _movieService.UpdateAsync(movie, token);
        if (updatedMovie is null)
        {
            return NotFound();
        }   
        
        var response = updatedMovie.MapToResponse();
        return Ok(response);
    }

    [HttpDelete]
    [Route(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _movieService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}