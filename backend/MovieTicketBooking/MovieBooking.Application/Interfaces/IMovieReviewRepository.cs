using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IMovieReviewRepository
{

    Task<List<MovieReview>> GetByMovieAsync(Guid movieId, bool approvedOnly = true);
    Task UpdateAsync(MovieReview review);
    Task<MovieReview?> GetByIdAsync(Guid id);

    Task DeleteAsync(MovieReview review);
     Task AddAsync(MovieReview review);
}
