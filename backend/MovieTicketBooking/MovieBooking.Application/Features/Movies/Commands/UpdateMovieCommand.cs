using MediatR;
using MovieBooking.Application.DTOs.Movies;

namespace MovieBooking.Application.Features.Movies.Commands;

public record UpdateMovieCommand(
    Guid Id,
    string? Title,
    string? Description,
    int? DurationMinutes,
    DateTime? ReleaseDate,
    string? Director,
    string? Cast,
    string? CertificateRating,
    bool? IsActive
) : IRequest<MovieDto>;
