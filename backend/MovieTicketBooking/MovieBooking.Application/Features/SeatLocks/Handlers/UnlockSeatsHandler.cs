using MediatR;
using MovieBooking.Application.Features.SeatLocks.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.Features.SeatLocks.Handlers;

public class UnlockSeatsHandler : IRequestHandler<UnlockSeatsCommand, bool>
{
    private readonly IShowtimeSeatRepository _showtimeSeatRepository;
    private readonly ISeatAvailabilityNotifier _notifier;

    public UnlockSeatsHandler(IShowtimeSeatRepository showtimeSeatRepository, ISeatAvailabilityNotifier notifier)
    {
        _showtimeSeatRepository = showtimeSeatRepository;
        _notifier = notifier;
    }

    public async Task<bool> Handle(UnlockSeatsCommand request, CancellationToken cancellationToken)
    {
        var seats = await _showtimeSeatRepository.GetByIdsAsync(request.SeatIds);

        var toUnlock = seats.Where(s =>
            s.Status == SeatStatus.Reserved &&
            s.LockedBySession == request.SessionId).ToList();

        foreach (var seat in toUnlock)
        {
            seat.Status = SeatStatus.Available;
            seat.BlockedUntil = null;
            seat.LockedBySession = null;
        }

        if (toUnlock.Any())
        {
            await _showtimeSeatRepository.UpdateRangeAsync(toUnlock);
            foreach (var seat in toUnlock)
                await _notifier.NotifySeatStatusChangedAsync(request.ShowtimeId, seat.SeatId, "Available");
        }

        return true;
    }
}
