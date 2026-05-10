namespace MovieBooking.Application.Interfaces;

public interface IBookingSettings
{
    int LockDurationMinutes { get; }
    int ExpiryMinutes { get; }
}
