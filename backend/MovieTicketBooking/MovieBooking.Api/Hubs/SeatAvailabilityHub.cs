using Microsoft.AspNetCore.SignalR;

namespace MovieBooking.Api.Hubs;

public class SeatAvailabilityHub : Hub
{
    public async Task JoinShowtime(string showtimeId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"showtime-{showtimeId}");
    }

    public async Task LeaveShowtime(string showtimeId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"showtime-{showtimeId}");
    }
}
