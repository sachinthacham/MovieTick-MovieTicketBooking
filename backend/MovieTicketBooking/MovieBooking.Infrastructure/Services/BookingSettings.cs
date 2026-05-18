using Microsoft.Extensions.Configuration;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Infrastructure.Services;

public class BookingSettings : IBookingSettings
{
    public int LockDurationMinutes { get; }
    public int ExpiryMinutes { get; }

    public BookingSettings(IConfiguration config)
    {
        LockDurationMinutes = config.GetValue<int>("Booking:LockDurationMinutes", 10);
        ExpiryMinutes = config.GetValue<int>("Booking:ExpiryMinutes", 15);
    }
}
