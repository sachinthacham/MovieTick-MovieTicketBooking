using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IScreenRepository
{
    Task AddAsync(Screen screen);
    Task<List<Screen>> GetByTheaterAsync(Guid theaterId);
    Task<Screen?> GetByIdAsync(Guid id);
    Task UpdateAsync(Screen screen);
    Task DeleteAsync(Screen screen);
}
