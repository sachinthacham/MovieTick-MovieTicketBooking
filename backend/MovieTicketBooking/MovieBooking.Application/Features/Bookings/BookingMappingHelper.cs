using MovieBooking.Application.DTOs.Bookings;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Bookings;

public static class BookingMappingHelper
{
    public static BookingDto ToDto(Booking booking, Dictionary<Guid, string> ticketMap)
    {
        return new BookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            UserName = booking.User?.FullName ?? string.Empty,
            UserEmail = booking.User?.Email ?? string.Empty,
            ShowtimeId = booking.ShowtimeId,
            MovieTitle = booking.Showtime?.Movie?.Title ?? string.Empty,
            TheaterName = booking.Showtime?.Screen?.Theater?.Name ?? string.Empty,
            ScreenName = booking.Showtime?.Screen?.Name ?? string.Empty,
            ShowtimeStart = booking.Showtime?.StartTime ?? default,
            BookingReference = booking.BookingReference,
            Status = booking.Status,
            TotalAmount = booking.TotalAmount,
            ExpiresAt = booking.ExpiresAt,
            ConfirmedAt = booking.ConfirmedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason,
            CreatedAt = booking.CreatedAt,
            Items = booking.BookingItems?.Select(item => new BookingItemDto
            {
                Id = item.Id,
                ShowtimeSeatId = item.ShowtimeSeatId,
                SeatNumber = item.SeatNumber,
                RowLabel = item.RowLabel,
                SeatCategoryName = item.SeatCategoryName,
                Price = item.Price,
                TicketNumber = ticketMap.TryGetValue(item.Id, out var tn) ? tn : null
            }).ToList() ?? [],
            Payment = booking.Payment == null ? null : new PaymentDto
            {
                Id = booking.Payment.Id,
                StripePaymentIntentId = booking.Payment.StripePaymentIntentId,
                Amount = booking.Payment.Amount,
                Currency = booking.Payment.Currency,
                Status = booking.Payment.Status,
                FailureReason = booking.Payment.FailureReason,
                PaidAt = booking.Payment.PaidAt
            }
        };
    }
}
