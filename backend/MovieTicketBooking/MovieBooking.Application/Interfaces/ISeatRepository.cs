using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface ISeatRepository
{
    Task AddRangeAsync(IEnumerable<Seat> seats);
    Task<List<Seat>> GetByScreenAsync(Guid screenId);
    Task<Seat?> GetByIdAsync(Guid id);
    Task UpdateAsync(Seat seat);
    Task UpdateRangeAsync(IEnumerable<Seat> seats);
    Task<List<Seat>> GetActiveByScreenAsync(Guid screenId);
}
