using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IMoviePosterRepository
{
    Task AddAsync(MoviePoster poster);
    Task<MoviePoster?> GetByIdAsync(Guid id);
    Task<List<MoviePoster>> GetByMovieAsync(Guid movieId);
    Task UpdateAsync(MoviePoster poster);
    Task DeleteAsync(MoviePoster poster);
    Task ClearPrimaryAsync(Guid movieId);
}
