using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class Seat : BaseEntity
{
    public Guid ScreenId { get; set; }
    public Screen Screen { get; set; } = default!;

    public Guid SeatCategoryId { get; set; }
    public SeatCategory SeatCategory { get; set; } = default!;

    public string SeatNumber { get; set; } = default!;
    public string Row { get; set; } = default!;
    public int Column { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<ShowtimeSeat> ShowtimeSeats { get; set; } = [];
}
