using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class Movie : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DurationMinutes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; }
    public string? CertificateRating { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsComingSoon { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal? AverageRating { get; set; }
    public int TotalRatings { get; set; }

    public ICollection<MovieGenre> Genres { get; set; } = [];
    public ICollection<MovieLanguage> Languages { get; set; } = [];
    public ICollection<MovieTrailer> Trailers { get; set; } = [];
    public ICollection<MoviePoster> Posters { get; set; } = [];
    public ICollection<MovieRating> Ratings { get; set; } = [];
    public ICollection<MovieReview> Reviews { get; set; } = [];
    public ICollection<Showtime> Showtimes { get; set; } = [];
}