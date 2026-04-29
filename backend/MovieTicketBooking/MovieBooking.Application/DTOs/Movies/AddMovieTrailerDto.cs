namespace MovieBooking.Application.DTOs.Movies;

public class AddMovieTrailerDto
{
    public string Title { get; set; } = default!;
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
