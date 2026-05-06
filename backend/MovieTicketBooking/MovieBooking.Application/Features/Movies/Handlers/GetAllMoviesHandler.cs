using MediatR;
using MovieBooking.Application.DTOs.Movies;
using MovieBooking.Application.Features.Movies.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, PagedMoviesDto>
{
    private readonly IMovieRepository _movieRepository;

    public GetAllMoviesHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<PagedMoviesDto> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _movieRepository.GetAllAsync(
            request.Page, request.PageSize,
            request.Search, request.GenreId, request.LanguageId,
            request.IsFeatured, request.IsComingSoon, request.IsActive,
            request.SortBy, request.CityFilter, request.FormatId);

        return new PagedMoviesDto
        {
            Items = items.Select(MovieMappingHelper.ToDto).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
