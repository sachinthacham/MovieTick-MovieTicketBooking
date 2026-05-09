using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface ITheaterImageRepository
{
    Task AddAsync(TheaterImage image);
    Task<TheaterImage?> GetByIdAsync(Guid id);
    Task<List<TheaterImage>> GetByTheaterAsync(Guid theaterId);
    Task UpdateAsync(TheaterImage image);
    Task DeleteAsync(TheaterImage image);
    Task ClearPrimaryAsync(Guid theaterId);
}
