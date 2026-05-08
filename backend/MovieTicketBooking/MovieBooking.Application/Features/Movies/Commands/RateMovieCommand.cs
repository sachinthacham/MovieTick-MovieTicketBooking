using MediatR;

namespace MovieBooking.Application.Features.Movies.Commands;

public record RateMovieCommand(Guid MovieId, Guid UserId, decimal Rating) : IRequest<bool>;
