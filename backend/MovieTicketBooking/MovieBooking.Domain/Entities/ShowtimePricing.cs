using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class ShowtimePricing : BaseEntity
{
    public Guid ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = default!;

    public Guid SeatCategoryId { get; set; }
    public SeatCategory SeatCategory { get; set; } = default!;

    public decimal Price { get; set; }
}
