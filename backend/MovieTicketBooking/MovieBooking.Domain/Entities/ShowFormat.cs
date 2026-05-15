using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class ShowFormat : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public ICollection<Showtime> Showtimes { get; set; } = [];
}
