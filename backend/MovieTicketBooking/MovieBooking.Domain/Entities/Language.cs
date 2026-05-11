using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class Language : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;

    public ICollection<MovieLanguage> Movies { get; set; } = [];
    public ICollection<Showtime> Showtimes { get; set; } = [];
}
