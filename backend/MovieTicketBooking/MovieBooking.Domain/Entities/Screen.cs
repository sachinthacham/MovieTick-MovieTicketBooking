using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class Screen : BaseEntity
{
    public Guid TheaterId { get; set; }
    public Theater Theater { get; set; } = default!;

    public string Name { get; set; } = default!;
    public int TotalSeats { get; set; }
    public string ScreenType { get; set; } = "Standard";
    public bool IsActive { get; set; } = true;

    public ICollection<Seat> Seats { get; set; } = [];
    public ICollection<Showtime> Showtimes { get; set; } = [];
}
