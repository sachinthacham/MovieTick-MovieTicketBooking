using Microsoft.AspNetCore.SignalR;
using MovieBooking.Api.Hubs;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Api.Services;

public class SignalRSeatAvailabilityNotifier : ISeatAvailabilityNotifier
{
    private readonly IHubContext<SeatAvailabilityHub> _hubContext;

    public SignalRSeatAvailabilityNotifier(IHubContext<SeatAvailabilityHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifySeatStatusChangedAsync(Guid showtimeId, Guid seatId, string status)
    {
        await _hubContext.Clients.Group($"showtime-{showtimeId}")
            .SendAsync("SeatStatusChanged", new { showtimeId, seatId, status });
    }
}
