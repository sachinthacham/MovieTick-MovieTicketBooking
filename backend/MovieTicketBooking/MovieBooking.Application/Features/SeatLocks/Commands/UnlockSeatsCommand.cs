using MediatR;

namespace MovieBooking.Application.Features.SeatLocks.Commands;

public record UnlockSeatsCommand(Guid ShowtimeId, List<Guid> SeatIds, string SessionId) : IRequest<bool>;
