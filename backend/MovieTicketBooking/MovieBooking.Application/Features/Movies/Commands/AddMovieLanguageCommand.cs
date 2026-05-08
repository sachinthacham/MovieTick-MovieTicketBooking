using MediatR;

namespace MovieBooking.Application.Features.Movies.Commands;

public record AddMovieLanguageCommand(Guid MovieId, Guid LanguageId) : IRequest<bool>;
public record RemoveMovieLanguageCommand(Guid MovieId, Guid LanguageId) : IRequest<bool>;
