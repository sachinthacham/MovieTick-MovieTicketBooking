namespace MovieBooking.Application.DTOs.Movies;

public class UpdateMovieDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; }
    public string? CertificateRating { get; set; }
    public bool? IsActive { get; set; }
}
