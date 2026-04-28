using MediatR;

namespace MovieBooking.Application.Features.Admin.Commands;

public record DeactivateUserCommand(Guid UserId) : IRequest<bool>;

public record ActivateUserCommand(Guid UserId) : IRequest<bool>;
