using MediatR;
using MovieBooking.Application.DTOs.Tickets;
using MovieBooking.Application.Features.Tickets.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Tickets.Handlers;

public class GetTicketsByBookingHandler : IRequestHandler<GetTicketsByBookingQuery, List<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IBookingRepository _bookingRepository;

    public GetTicketsByBookingHandler(ITicketRepository ticketRepository, IBookingRepository bookingRepository)
    {
        _ticketRepository = ticketRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<List<TicketDto>> Handle(GetTicketsByBookingQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId)
            ?? throw new KeyNotFoundException("Booking not found.");

        if (request.RequestingUserRole != "Admin" && booking.UserId != request.RequestingUserId)
            throw new UnauthorizedAccessException("Access denied.");

        var tickets = await _ticketRepository.GetByBookingAsync(request.BookingId);

        return tickets.Select(t => new TicketDto
        {
            Id = t.Id,
            BookingId = t.BookingId,
            BookingReference = booking.BookingReference,
            TicketNumber = t.TicketNumber,
            QrCodeData = t.QrCodeData,
            IsUsed = t.IsUsed,
            UsedAt = t.UsedAt,
            SeatNumber = t.BookingItem?.SeatNumber ?? string.Empty,
            RowLabel = t.BookingItem?.RowLabel ?? string.Empty,
            SeatCategoryName = t.BookingItem?.SeatCategoryName ?? string.Empty,
            Price = t.BookingItem?.Price ?? 0,
            MovieTitle = booking.Showtime?.Movie?.Title ?? string.Empty,
            TheaterName = booking.Showtime?.Screen?.Theater?.Name ?? string.Empty,
            ScreenName = booking.Showtime?.Screen?.Name ?? string.Empty,
            ShowtimeStart = booking.Showtime?.StartTime ?? default
        }).ToList();
    }
}
