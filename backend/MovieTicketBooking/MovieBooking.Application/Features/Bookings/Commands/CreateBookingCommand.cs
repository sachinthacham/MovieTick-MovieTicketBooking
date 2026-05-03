using MediatR;
using MovieBooking.Application.DTOs.Bookings;

namespace MovieBooking.Application.Features.Bookings.Commands;

public record CreateBookingCommand(
    Guid UserId,
    Guid ShowtimeId,
    List<Guid> SeatIds,
    string SessionId
) : IRequest<CreateBookingResultDto>;

public class CreateBookingResultDto
{
    public Guid BookingId { get; set; }
    public string BookingReference { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public string StripeClientSecret { get; set; } = default!;
    public string StripePaymentIntentId { get; set; } = default!;
}
