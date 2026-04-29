using MovieBooking.Application.DTOs.Genres;
using MovieBooking.Application.DTOs.Languages;

namespace MovieBooking.Application.DTOs.Movies;

public class MovieDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DurationMinutes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; }
    public string? CertificateRating { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsComingSoon { get; set; }
    public bool IsActive { get; set; }
    public decimal? AverageRating { get; set; }
    public int TotalRatings { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GenreDto> Genres { get; set; } = [];
    public List<LanguageDto> Languages { get; set; } = [];
    public List<MoviePosterDto> Posters { get; set; } = [];
    public List<MovieTrailerDto> Trailers { get; set; } = [];
}

public class MoviePosterDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = default!;
    public bool IsPrimary { get; set; }
    public string PosterType { get; set; } = default!;
}

public class MovieTrailerDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
