using MediatR;
using MovieBooking.Application.DTOs.Auth;

namespace MovieBooking.Application.Features.Auth.Commands;

public record LoginUserCommand(string Email, string Password) : IRequest<AuthResponseDto>;