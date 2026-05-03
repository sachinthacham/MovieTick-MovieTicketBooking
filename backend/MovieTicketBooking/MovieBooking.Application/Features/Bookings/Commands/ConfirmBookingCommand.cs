using MediatR;

namespace MovieBooking.Application.Features.Bookings.Commands;

public record ConfirmBookingCommand(string PaymentIntentId, string? ChargeId) : IRequest<bool>;
