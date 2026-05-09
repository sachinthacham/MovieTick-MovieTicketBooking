using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface ITheaterRepository
{
    Task AddAsync(Theater theater);
    Task<List<Theater>> GetAllAsync(string? city, bool? isActive);
    Task<Theater?> GetByIdAsync(Guid id);
    Task UpdateAsync(Theater theater);
    Task DeleteAsync(Theater theater);
    Task<decimal> GetAverageRatingAsync(Guid theaterId);
    Task<List<string>> GetDistinctCitiesAsync();
}
