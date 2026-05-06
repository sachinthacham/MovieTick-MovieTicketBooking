using MediatR;

namespace MovieBooking.Application.Features.Movies.Commands;

public record CreateMovieCommand(
    string Title,
    string Description,
    int DurationMinutes,
    DateTime ReleaseDate,
    string? Director,
    string? Cast,
    string? CertificateRating,
    bool IsComingSoon,
    bool IsFeatured
) : IRequest<Guid>;
