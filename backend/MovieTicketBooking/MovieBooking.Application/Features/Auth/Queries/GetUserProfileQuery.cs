using MediatR;
using MovieBooking.Application.DTOs.Auth;

namespace MovieBooking.Application.Features.Auth.Queries;

public record GetUserProfileQuery(Guid UserId) : IRequest<UserProfileDto>;
