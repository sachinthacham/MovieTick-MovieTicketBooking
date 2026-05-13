using MovieBooking.Domain.Common;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Domain.Entities;

public class ShowtimeSeat : BaseEntity
{
    public Guid ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = default!;

    public Guid SeatId { get; set; }
    public Seat Seat { get; set; } = default!;

    public SeatStatus Status { get; set; } = SeatStatus.Available;
    public DateTime? BlockedUntil { get; set; }
    public string? LockedBySession { get; set; }

    public ICollection<BookingItem> BookingItems { get; set; } = [];
}
