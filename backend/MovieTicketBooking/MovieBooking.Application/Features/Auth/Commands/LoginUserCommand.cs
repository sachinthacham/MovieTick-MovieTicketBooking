using MediatR;

namespace MovieBooking.Application.Features.Auth.Commands;

public record LoginUserCommand(string Email, string Password) : IRequest<string>;