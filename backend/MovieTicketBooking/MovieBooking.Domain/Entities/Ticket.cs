using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class Ticket : BaseEntity
{
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; } = default!;

    public Guid BookingItemId { get; set; }
    public BookingItem BookingItem { get; set; } = default!;

    public string TicketNumber { get; set; } = default!;
    public string QrCodeData { get; set; } = default!;
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
}
