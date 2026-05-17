using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;
using MovieBooking.Domain.Enums;
using MovieBooking.Infrastructure.Data;

namespace MovieBooking.Infrastructure.Repositories;

public class ShowtimeSeatRepository : IShowtimeSeatRepository
{
    private readonly ApplicationDbContext _context;

    public ShowtimeSeatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(IEnumerable<ShowtimeSeat> seats)
    {
        await _context.ShowtimeSeats.AddRangeAsync(seats);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ShowtimeSeat>> GetByShowtimeAsync(Guid showtimeId)
        => await _context.ShowtimeSeats
            .Include(ss => ss.Seat).ThenInclude(s => s.SeatCategory)
            .Where(ss => ss.ShowtimeId == showtimeId)
            .ToListAsync();

    public async Task<ShowtimeSeat?> GetByShowtimeAndSeatAsync(Guid showtimeId, Guid seatId)
        => await _context.ShowtimeSeats
            .FirstOrDefaultAsync(ss => ss.ShowtimeId == showtimeId && ss.SeatId == seatId);

    public async Task UpdateAsync(ShowtimeSeat seat)
    {
        _context.ShowtimeSeats.Update(seat);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<ShowtimeSeat> seats)
    {
        _context.ShowtimeSeats.UpdateRange(seats);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ShowtimeSeat>> GetByIdsAsync(IEnumerable<Guid> ids)
        => await _context.ShowtimeSeats
            .Include(ss => ss.Seat).ThenInclude(s => s.SeatCategory)
            .Where(ss => ids.Contains(ss.Id))
            .ToListAsync();

    public async Task<List<ShowtimeSeat>> GetLockedExpiredAsync()
        => await _context.ShowtimeSeats
            .Where(ss => ss.Status == SeatStatus.Reserved && ss.BlockedUntil < DateTime.UtcNow)
            .ToListAsync();
}
