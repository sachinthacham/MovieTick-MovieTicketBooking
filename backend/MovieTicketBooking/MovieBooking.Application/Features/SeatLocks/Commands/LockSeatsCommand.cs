using MediatR;

namespace MovieBooking.Application.Features.SeatLocks.Commands;

public record LockSeatsCommand(Guid ShowtimeId, List<Guid> SeatIds, string SessionId) : IRequest<bool>;
