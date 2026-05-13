using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class BookingItem : BaseEntity
{
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; } = default!;

    public Guid ShowtimeSeatId { get; set; }
    public ShowtimeSeat ShowtimeSeat { get; set; } = default!;

    public string SeatNumber { get; set; } = default!;
    public string RowLabel { get; set; } = default!;
    public string SeatCategoryName { get; set; } = default!;
    public decimal Price { get; set; }

    public Ticket? Ticket { get; set; }
}
