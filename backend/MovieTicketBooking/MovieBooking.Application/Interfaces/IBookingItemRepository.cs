using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IBookingItemRepository
{
    Task AddRangeAsync(IEnumerable<BookingItem> items);
    Task<List<BookingItem>> GetByBookingAsync(Guid bookingId);
}
