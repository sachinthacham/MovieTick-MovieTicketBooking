using MediatR;

namespace MovieBooking.Application.Features.Admin.Commands;

public record UpdateUserRoleCommand(Guid UserId, string NewRole) : IRequest<bool>;
