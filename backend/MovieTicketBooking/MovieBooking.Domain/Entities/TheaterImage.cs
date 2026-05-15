using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class TheaterImage : BaseEntity
{
    public Guid TheaterId { get; set; }
    public Theater Theater { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
