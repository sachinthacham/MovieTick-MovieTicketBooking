using MediatR;
using MovieBooking.Application.DTOs.Auth;

namespace MovieBooking.Application.Features.Auth.Commands;

public record RequestPasswordResetCommand(string Email) : IRequest<PasswordResetResponseDto>;
