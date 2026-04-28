using MediatR;
using MovieBooking.Application.DTOs.Auth;
using MovieBooking.Application.Features.Auth.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Auth.Handlers;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        return new UserProfileDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            ProfileImageUrl = user.ProfileImageUrl,
            Bio = user.Bio,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
            SocialProvider = user.SocialProvider,
            CreatedAt = user.CreatedAt
        };
    }
}
