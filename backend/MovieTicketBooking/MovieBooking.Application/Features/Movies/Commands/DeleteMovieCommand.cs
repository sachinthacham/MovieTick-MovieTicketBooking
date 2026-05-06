using MediatR;

namespace MovieBooking.Application.Features.Movies.Commands;

public record DeleteMovieCommand(Guid Id) : IRequest<bool>;
