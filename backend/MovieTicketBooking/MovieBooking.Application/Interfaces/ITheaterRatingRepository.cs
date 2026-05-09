using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface ITheaterRatingRepository
{
    Task AddAsync(TheaterRating rating);
    Task<TheaterRating?> GetByUserAndTheaterAsync(Guid userId, Guid theaterId);
    Task<List<TheaterRating>> GetByTheaterAsync(Guid theaterId);
    Task UpdateAsync(TheaterRating rating);
    Task<decimal> GetAverageRatingAsync(Guid theaterId);
}
