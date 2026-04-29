using MediatR;
using MovieBooking.Application.DTOs.Genres;

namespace MovieBooking.Application.Features.Genres.Commands;

public record CreateGenreCommand(string Name) : IRequest<GenreDto>;
public record UpdateGenreCommand(Guid Id, string Name) : IRequest<GenreDto>;
public record DeleteGenreCommand(Guid Id) : IRequest<bool>;
