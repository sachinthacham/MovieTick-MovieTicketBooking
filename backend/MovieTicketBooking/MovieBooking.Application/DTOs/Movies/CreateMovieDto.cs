namespace MovieBooking.Application.DTOs.Movies;

public class CreateMovieDto
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DurationMinutes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; }
    public string? CertificateRating { get; set; }
    public bool IsComingSoon { get; set; }
    public bool IsFeatured { get; set; }
}
