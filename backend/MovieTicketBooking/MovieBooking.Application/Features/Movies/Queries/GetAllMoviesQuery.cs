using MediatR;
using MovieBooking.Application.DTOs.Movies;

namespace MovieBooking.Application.Features.Movies.Queries;

public record GetAllMoviesQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    Guid? GenreId = null,
    Guid? LanguageId = null,
    bool? IsFeatured = null,
    bool? IsComingSoon = null,
    bool? IsActive = null,
    string? SortBy = null,
    string? CityFilter = null,
    Guid? FormatId = null
) : IRequest<PagedMoviesDto>;
