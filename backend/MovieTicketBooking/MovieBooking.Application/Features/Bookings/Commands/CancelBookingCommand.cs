using MediatR;

namespace MovieBooking.Application.Features.Bookings.Commands;

public record CancelBookingCommand(Guid BookingId, Guid RequestingUserId, string RequestingUserRole, string? Reason) : IRequest<bool>;
