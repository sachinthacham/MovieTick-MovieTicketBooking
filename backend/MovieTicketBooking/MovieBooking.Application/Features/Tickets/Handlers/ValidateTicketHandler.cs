using MediatR;
using MovieBooking.Application.DTOs.Tickets;
using MovieBooking.Application.Features.Tickets.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Tickets.Handlers;

public class ValidateTicketHandler : IRequestHandler<ValidateTicketCommand, TicketDto>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IBookingRepository _bookingRepository;

    public ValidateTicketHandler(ITicketRepository ticketRepository, IBookingRepository bookingRepository)
    {
        _ticketRepository = ticketRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<TicketDto> Handle(ValidateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByNumberAsync(request.TicketNumber)
            ?? throw new KeyNotFoundException("Ticket not found.");

        if (ticket.IsUsed)
            throw new InvalidOperationException("Ticket has already been used.");

        ticket.IsUsed = true;
        ticket.UsedAt = DateTime.UtcNow;
        await _ticketRepository.UpdateAsync(ticket);

        var booking = await _bookingRepository.GetByIdAsync(ticket.BookingId);

        return new TicketDto
        {
            Id = ticket.Id,
            BookingId = ticket.BookingId,
            BookingReference = booking?.BookingReference ?? string.Empty,
            TicketNumber = ticket.TicketNumber,
            QrCodeData = ticket.QrCodeData,
            IsUsed = ticket.IsUsed,
            UsedAt = ticket.UsedAt,
            SeatNumber = ticket.BookingItem?.SeatNumber ?? string.Empty,
            RowLabel = ticket.BookingItem?.RowLabel ?? string.Empty,
            SeatCategoryName = ticket.BookingItem?.SeatCategoryName ?? string.Empty,
            Price = ticket.BookingItem?.Price ?? 0,
            MovieTitle = booking?.Showtime?.Movie?.Title ?? string.Empty,
            TheaterName = booking?.Showtime?.Screen?.Theater?.Name ?? string.Empty,
            ScreenName = booking?.Showtime?.Screen?.Name ?? string.Empty,
            ShowtimeStart = booking?.Showtime?.StartTime ?? default
        };
    }
}
