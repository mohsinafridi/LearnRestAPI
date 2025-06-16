using Movies.Application.Models;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public  Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            return _movieRepository.CreateAsync(movie);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _movieRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
        {
            return _movieRepository.GetAllAsync();
        }

        public Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _movieRepository.GetByIdAsync(id);
        }

        public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            return _movieRepository.GetByIdSlug(slug);
        }
        public async  Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default)
        {
            var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id, token);
            if (!movieExists)
            {
                return null;
            }

            await _movieRepository.UpdateAsync(movie , token);
            return movie;

        }
    }
}
