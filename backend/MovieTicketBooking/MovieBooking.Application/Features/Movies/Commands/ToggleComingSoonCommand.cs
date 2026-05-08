using MediatR;

namespace MovieBooking.Application.Features.Movies.Commands;

public record ToggleComingSoonCommand(Guid MovieId, bool IsComingSoon) : IRequest<bool>;
