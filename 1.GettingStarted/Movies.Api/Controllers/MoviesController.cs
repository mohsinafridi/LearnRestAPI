﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;


[ApiController]
[ApiVersion(1.0)]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    [Route(ApiEndpoints.Movies.Create)]
    //  [Authorize(AuthConstants.TrustedMemberPolicyName)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request , CancellationToken token)
    {
        var movie = request.MapToMovie();
        var result = await _movieService.CreateAsync(movie, token);
        return CreatedAtAction(nameof(GetV1),new { idOrSlug = movie.Id }, movie);
    }

     
    [HttpGet(ApiEndpoints.Movies.Get)]    
    public async Task<IActionResult> GetV1([FromRoute] string idOrSlug, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var movie = Guid.TryParse(idOrSlug, out var id)
           ? await _movieService.GetByIdAsync(id, userId, token)
           : await _movieService.GetBySlugAsync(idOrSlug, userId, token);
        if (movie == null)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);
    }

   

    [HttpGet(ApiEndpoints.Movies.GetAll)]    
    public async Task<IActionResult> GetAll([FromQuery]GetAllMovieRequest request,CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var options = request.MapToOption().WithUser(userId);

        var movies = await _movieService.GetAllAsync(options, token);

        var moviesCount = await _movieService.GetCountAsync(options.Title,options.YearOfRelease, token);
        var moviesResponse = movies.MapToResponse(request.Page,request.PageSize,moviesCount);

        return Ok(moviesResponse);
    }


    [HttpPut(ApiEndpoints.Movies.Update)]
    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movie =  request.MapToMovie(id);

      var updatedMovie =  await _movieService.UpdateAsync(movie, userId, token);
        if (updatedMovie is null)
        {
            return NotFound();
        }   
        
        var response = updatedMovie.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]    
    [Authorize(AuthConstants.AdminUserPolicyName)]
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