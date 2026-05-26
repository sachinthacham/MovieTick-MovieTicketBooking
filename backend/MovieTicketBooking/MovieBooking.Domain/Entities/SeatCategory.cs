using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class SeatCategory : BaseEntity
{
    public string Name { get; set; } = default!;
    public decimal DefaultPrice { get; set; }
    public string? Description { get; set; }
    public string Color { get; set; } = "#000000";
   

    public ICollection<ShowtimePricing> ShowtimePricings { get; set; } = [];
    public ICollection<Seat> Seats { get; set; } = [];
    
}
