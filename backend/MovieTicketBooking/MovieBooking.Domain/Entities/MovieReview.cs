using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class MovieReview : BaseEntity
{
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public string ReviewText { get; set; } = default!;
    public bool IsApproved { get; set; }
}
