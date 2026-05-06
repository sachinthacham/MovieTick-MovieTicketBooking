using MediatR;
using MovieBooking.Application.DTOs.Bookings;
using MovieBooking.Application.Features.Bookings.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Bookings.Handlers;

public class GetBookingByIdHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITicketRepository _ticketRepository;

    public GetBookingByIdHandler(IBookingRepository bookingRepository, ITicketRepository ticketRepository)
    {
        _bookingRepository = bookingRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId)
            ?? throw new KeyNotFoundException("Booking not found.");

        if (request.RequestingUserRole != "Admin" && booking.UserId != request.RequestingUserId)
            throw new UnauthorizedAccessException("Access denied.");

        var tickets = await _ticketRepository.GetByBookingAsync(booking.Id);
        var ticketMap = tickets.ToDictionary(t => t.BookingItemId, t => t.TicketNumber);

        return BookingMappingHelper.ToDto(booking, ticketMap);
    }
}
