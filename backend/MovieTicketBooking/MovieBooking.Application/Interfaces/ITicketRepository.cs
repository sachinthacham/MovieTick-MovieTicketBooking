using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface ITicketRepository
{
    Task AddRangeAsync(IEnumerable<Ticket> tickets);
    Task<List<Ticket>> GetByBookingAsync(Guid bookingId);
    Task<Ticket?> GetByNumberAsync(string ticketNumber);
    Task UpdateAsync(Ticket ticket);
}
