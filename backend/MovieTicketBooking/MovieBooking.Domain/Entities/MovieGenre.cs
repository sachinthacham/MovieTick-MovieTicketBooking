namespace MovieBooking.Domain.Entities;

public class MovieGenre
{
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = default!;

    public Guid GenreId { get; set; }
    public Genre Genre { get; set; } = default!;
}
