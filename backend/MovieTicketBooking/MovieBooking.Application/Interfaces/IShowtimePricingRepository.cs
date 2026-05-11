using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IShowtimePricingRepository
{
    Task AddAsync(ShowtimePricing pricing);
    Task<List<ShowtimePricing>> GetByShowtimeAsync(Guid showtimeId);
    Task<ShowtimePricing?> GetByShowtimeAndCategoryAsync(Guid showtimeId, Guid seatCategoryId);
    Task UpdateAsync(ShowtimePricing pricing);
    Task DeleteAsync(ShowtimePricing pricing);
}
