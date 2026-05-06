using MovieBooking.Application.DTOs.Genres;
using MovieBooking.Application.DTOs.Languages;
using MovieBooking.Application.DTOs.Movies;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Movies.Handlers;

internal static class MovieMappingHelper
{
    internal static MovieDto ToDto(Movie movie) => new()
    {
        Id = movie.Id,
        Title = movie.Title,
        Description = movie.Description,
        DurationMinutes = movie.DurationMinutes,
        ReleaseDate = movie.ReleaseDate,
        Director = movie.Director,
        Cast = movie.Cast,
        CertificateRating = movie.CertificateRating,
        IsFeatured = movie.IsFeatured,
        IsComingSoon = movie.IsComingSoon,
        IsActive = movie.IsActive,
        AverageRating = movie.AverageRating,
        TotalRatings = movie.TotalRatings,
        CreatedAt = movie.CreatedAt,
        Genres = movie.Genres.Select(g => new GenreDto { Id = g.Genre.Id, Name = g.Genre.Name }).ToList(),
        Languages = movie.Languages.Select(l => new LanguageDto { Id = l.Language.Id, Name = l.Language.Name, Code = l.Language.Code }).ToList(),
        Posters = movie.Posters.Select(p => new MoviePosterDto { Id = p.Id, ImageUrl = p.ImageUrl, IsPrimary = p.IsPrimary, PosterType = p.PosterType.ToString() }).ToList(),
        Trailers = movie.Trailers.Select(t => new MovieTrailerDto { Id = t.Id, Title = t.Title, Url = t.Url, IsPrimary = t.IsPrimary }).ToList()
    };
}
