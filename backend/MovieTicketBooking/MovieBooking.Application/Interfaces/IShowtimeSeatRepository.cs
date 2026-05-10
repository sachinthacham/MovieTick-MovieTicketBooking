using MovieBooking.Domain.Entities;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.Interfaces;

public interface IShowtimeSeatRepository
{
    Task AddRangeAsync(IEnumerable<ShowtimeSeat> seats);
    Task<List<ShowtimeSeat>> GetByShowtimeAsync(Guid showtimeId);
    Task<ShowtimeSeat?> GetByShowtimeAndSeatAsync(Guid showtimeId, Guid seatId);
    Task UpdateAsync(ShowtimeSeat seat);
    Task UpdateRangeAsync(IEnumerable<ShowtimeSeat> seats);
    Task<List<ShowtimeSeat>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<List<ShowtimeSeat>> GetLockedExpiredAsync();
}
