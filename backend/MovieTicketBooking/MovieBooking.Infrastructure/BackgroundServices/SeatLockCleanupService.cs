using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Infrastructure.BackgroundServices;

public class SeatLockCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeatLockCleanupService> _logger;

    public SeatLockCleanupService(IServiceProvider serviceProvider, ILogger<SeatLockCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SeatLockCleanupService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                await CleanupExpiredLocksAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SeatLockCleanupService");
            }
        }

        _logger.LogInformation("SeatLockCleanupService stopped.");
    }

    private async Task CleanupExpiredLocksAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var showtimeSeatRepo = scope.ServiceProvider.GetRequiredService<IShowtimeSeatRepository>();

        var expiredSeats = await showtimeSeatRepo.GetLockedExpiredAsync();

        if (expiredSeats.Count == 0) return;

        _logger.LogInformation("Releasing {Count} expired seat locks.", expiredSeats.Count);

        foreach (var seat in expiredSeats)
        {
            seat.Status = SeatStatus.Available;
            seat.BlockedUntil = null;
            seat.LockedBySession = null;
        }

        await showtimeSeatRepo.UpdateRangeAsync(expiredSeats);

        _logger.LogInformation("Expired seat locks released.");
    }
}
