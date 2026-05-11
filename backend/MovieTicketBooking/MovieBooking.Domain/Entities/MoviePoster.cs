using MovieBooking.Domain.Common;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Domain.Entities;

public class MoviePoster : BaseEntity
{
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;
    public bool IsPrimary { get; set; }
    public PosterType PosterType { get; set; } = PosterType.Portrait;
}
