using MediatR;
using MovieBooking.Application.DTOs.Genres;

namespace MovieBooking.Application.Features.Genres.Queries;

public record GetAllGenresQuery : IRequest<List<GenreDto>>;
public record GetGenreByIdQuery(Guid Id) : IRequest<GenreDto>;
