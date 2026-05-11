using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IMovieRatingRepository
{
    Task AddAsync(MovieRating rating);
    Task<MovieRating?> GetByUserAndMovieAsync(Guid userId, Guid movieId);
    Task<List<MovieRating>> GetByMovieAsync(Guid movieId);
    Task UpdateAsync(MovieRating rating);
    Task<decimal> GetAverageRatingAsync(Guid movieId);
    Task<int> GetTotalRatingsAsync(Guid movieId);
}
