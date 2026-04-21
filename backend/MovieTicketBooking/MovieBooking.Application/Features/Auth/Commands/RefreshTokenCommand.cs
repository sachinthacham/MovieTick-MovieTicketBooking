using MediatR;
using MovieBooking.Application.DTOs.Auth;

namespace MovieBooking.Application.Features.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;
