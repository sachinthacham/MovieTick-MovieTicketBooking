using MediatR;

namespace MovieBooking.Application.Features.Auth.Commands;

public record RegisterUserCommand(string FullName, string Email, string Password) : IRequest<Guid>;