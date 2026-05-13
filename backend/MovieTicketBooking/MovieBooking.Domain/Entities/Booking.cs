using MovieBooking.Domain.Common;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = default!;

    public string BookingReference { get; set; } = default!;
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public decimal TotalAmount { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }

    public ICollection<BookingItem> BookingItems { get; set; } = [];
    public Payment? Payment { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = [];
}
