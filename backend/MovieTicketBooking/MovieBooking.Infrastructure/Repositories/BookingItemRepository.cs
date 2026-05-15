using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;
using MovieBooking.Infrastructure.Data;

namespace MovieBooking.Infrastructure.Repositories;

public class BookingItemRepository : IBookingItemRepository
{
    private readonly ApplicationDbContext _context;

    public BookingItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(IEnumerable<BookingItem> items)
    {
        await _context.BookingItems.AddRangeAsync(items);
        await _context.SaveChangesAsync();
    }

    public async Task<List<BookingItem>> GetByBookingAsync(Guid bookingId)
        => await _context.BookingItems
            .Where(bi => bi.BookingId == bookingId)
            .ToListAsync();
}
