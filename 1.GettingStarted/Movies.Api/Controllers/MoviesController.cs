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
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        var result = await _movieService.CreateAsync(movie);
        return CreatedAtAction(nameof(Get),new { id = movie.Id }, movie);
    }


    [HttpGet]
    [Route(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpGet]
    [Route(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieService.GetAllAsync();

        var moviesResponse = movies.MapToResponse();

        return Ok(moviesResponse);
    }


    [HttpPut]
    [Route(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie =  request.MapToMovie(id);

      var updatedMovie =  await _movieService.UpdateAsync(movie);
        if (updatedMovie is null)
        {
            return NotFound();
        }   
        
        var response = updatedMovie.MapToResponse();
        return Ok(response);
    }

    [HttpDelete]
    [Route(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _movieService.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}