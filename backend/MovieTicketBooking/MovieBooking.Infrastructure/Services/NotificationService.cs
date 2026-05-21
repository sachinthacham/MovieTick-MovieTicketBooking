using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IEmailService _emailService;

    public NotificationService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendBookingConfirmationAsync(Booking booking)
    {
        var userEmail = booking.User?.Email;
        if (string.IsNullOrWhiteSpace(userEmail)) return;

        var subject = $"Booking Confirmed - {booking.BookingReference}";
        var body = $"""
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                <h2 style="color: #e50914;">Booking Confirmed!</h2>
                <p>Hi {booking.User?.FullName},</p>
                <p>Your booking has been confirmed.</p>
                <table style="border-collapse: collapse; width: 100%;">
                    <tr><td style="padding: 8px; font-weight: bold;">Booking Reference:</td><td>{booking.BookingReference}</td></tr>
                    <tr><td style="padding: 8px; font-weight: bold;">Movie:</td><td>{booking.Showtime?.Movie?.Title}</td></tr>
                    <tr><td style="padding: 8px; font-weight: bold;">Theater:</td><td>{booking.Showtime?.Screen?.Theater?.Name}</td></tr>
                    <tr><td style="padding: 8px; font-weight: bold;">Show Time:</td><td>{booking.Showtime?.StartTime:dddd, MMMM dd, yyyy hh:mm tt}</td></tr>
                    <tr><td style="padding: 8px; font-weight: bold;">Seats:</td><td>{string.Join(", ", booking.BookingItems?.Select(i => $"{i.RowLabel}{i.SeatNumber}") ?? [])}</td></tr>
                    <tr><td style="padding: 8px; font-weight: bold;">Total Amount:</td><td>${booking.TotalAmount:F2}</td></tr>
                </table>
                <p style="margin-top: 20px;">Please show your e-tickets at the venue. Enjoy the show!</p>
            </body>
            </html>
            """;

        await _emailService.SendEmailAsync(userEmail, subject, body);
    }

    public async Task SendCancellationAsync(Booking booking)
    {
        var userEmail = booking.User?.Email;
        if (string.IsNullOrWhiteSpace(userEmail)) return;

        var subject = $"Booking Cancelled - {booking.BookingReference}";
        var body = $"""
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                <h2 style="color: #e50914;">Booking Cancelled</h2>
                <p>Hi {booking.User?.FullName},</p>
                <p>Your booking has been cancelled.</p>
                <table style="border-collapse: collapse; width: 100%;">
                    <tr><td style="padding: 8px; font-weight: bold;">Booking Reference:</td><td>{booking.BookingReference}</td></tr>
                    <tr><td style="padding: 8px; font-weight: bold;">Movie:</td><td>{booking.Showtime?.Movie?.Title}</td></tr>
                    <tr><td style="padding: 8px; font-weight: bold;">Cancellation Reason:</td><td>{booking.CancellationReason ?? "Not specified"}</td></tr>
                </table>
                <p style="margin-top: 20px;">If a refund is applicable, it will be processed within 5-7 business days.</p>
            </body>
            </html>
            """;

        await _emailService.SendEmailAsync(userEmail, subject, body);
    }
}
