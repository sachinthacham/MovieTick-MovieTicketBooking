using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class MovieTrailer : BaseEntity
{
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
