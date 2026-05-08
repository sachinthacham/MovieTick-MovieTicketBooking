using MediatR;

namespace MovieBooking.Application.Features.Movies.Commands;

public record AddMovieGenreCommand(Guid MovieId, Guid GenreId) : IRequest<bool>;
public record RemoveMovieGenreCommand(Guid MovieId, Guid GenreId) : IRequest<bool>;
