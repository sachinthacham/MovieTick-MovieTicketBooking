using MediatR;
using MovieBooking.Application.DTOs.Auth;
using MovieBooking.Application.Features.Auth.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Auth.Handlers;

public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileDto>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserProfileHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (!string.IsNullOrWhiteSpace(request.FullName)) user.FullName = request.FullName;
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber)) user.PhoneNumber = request.PhoneNumber;
        if (!string.IsNullOrWhiteSpace(request.Bio)) user.Bio = request.Bio;
        if (request.DateOfBirth.HasValue) user.DateOfBirth = request.DateOfBirth;
        if (!string.IsNullOrWhiteSpace(request.Gender)) user.Gender = request.Gender;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

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
