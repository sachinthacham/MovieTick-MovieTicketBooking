using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface ISeatCategoryRepository
{
    Task AddAsync(SeatCategory category);
    Task<List<SeatCategory>> GetAllAsync();
    Task<SeatCategory?> GetByIdAsync(Guid id);
    Task UpdateAsync(SeatCategory category);
    Task DeleteAsync(SeatCategory category);
}
