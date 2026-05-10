namespace MovieBooking.Application.Interfaces;

public interface ISeatAvailabilityNotifier
{
    Task NotifySeatStatusChangedAsync(Guid showtimeId, Guid seatId, string status);
}
