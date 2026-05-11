using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IMovieReviewRepository
{
    Task AddAsync(MovieReview review);
    Task<MovieReview?> GetByIdAsync(Guid id);
    Task<List<MovieReview>> GetByMovieAsync(Guid movieId, bool approvedOnly = true);
    Task UpdateAsync(MovieReview review);
    Task DeleteAsync(MovieReview review);
}
