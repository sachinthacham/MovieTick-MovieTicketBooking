using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface INotificationService
{
    Task SendBookingConfirmationAsync(Booking booking);
    Task SendCancellationAsync(Booking booking);
}
