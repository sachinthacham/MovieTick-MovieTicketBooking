using MediatR;
using MovieBooking.Application.DTOs.Auth;

namespace MovieBooking.Application.Features.Auth.Commands;

public record UpdateUserProfileCommand(
    Guid UserId,
    string? FullName,
    string? PhoneNumber,
    string? Bio,
    DateTime? DateOfBirth,
    string? Gender
) : IRequest<UserProfileDto>;
