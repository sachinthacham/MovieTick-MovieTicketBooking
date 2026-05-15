using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class TheaterRating : BaseEntity
{
    public Guid TheaterId { get; set; }
    public Theater Theater { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public decimal Rating { get; set; }
    public string? Comment { get; set; }
}
