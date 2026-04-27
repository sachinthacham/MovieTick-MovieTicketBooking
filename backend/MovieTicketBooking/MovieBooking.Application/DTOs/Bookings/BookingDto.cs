using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.DTOs.Bookings;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string UserEmail { get; set; } = default!;
    public Guid ShowtimeId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public string TheaterName { get; set; } = default!;
    public string ScreenName { get; set; } = default!;
    public DateTime ShowtimeStart { get; set; }
    public string BookingReference { get; set; } = default!;
    public BookingStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<BookingItemDto> Items { get; set; } = [];
    public PaymentDto? Payment { get; set; }
}

public class BookingItemDto
{
    public Guid Id { get; set; }
    public Guid ShowtimeSeatId { get; set; }
    public string SeatNumber { get; set; } = default!;
    public string RowLabel { get; set; } = default!;
    public string SeatCategoryName { get; set; } = default!;
    public decimal Price { get; set; }
    public string? TicketNumber { get; set; }
}

public class PaymentDto
{
    public Guid Id { get; set; }
    public string StripePaymentIntentId { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = default!;
    public PaymentStatus Status { get; set; }
    public string? FailureReason { get; set; }
    public DateTime? PaidAt { get; set; }
}

public class PagedBookingsDto
{
    public List<BookingDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
