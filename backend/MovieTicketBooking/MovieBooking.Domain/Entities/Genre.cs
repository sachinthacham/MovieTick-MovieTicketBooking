using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class Genre : BaseEntity
{
    public string Name { get; set; } = default!;

    public ICollection<MovieGenre> Movies { get; set; } = [];
}
