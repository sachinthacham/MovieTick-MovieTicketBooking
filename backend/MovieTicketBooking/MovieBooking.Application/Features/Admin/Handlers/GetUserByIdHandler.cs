using MediatR;
using MovieBooking.Application.DTOs.Admin;
using MovieBooking.Application.Features.Admin.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Admin.Handlers;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserAdminDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserAdminDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new KeyNotFoundException("User not found.");

        return new UserAdminDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            ProfileImageUrl = user.ProfileImageUrl,
            IsActive = user.IsActive,
            SocialProvider = user.SocialProvider,
            CreatedAt = user.CreatedAt,
            TotalBookings = user.Bookings?.Count ?? 0,
            TotalSpent = user.Bookings?.Sum(b => b.TotalAmount) ?? 0
        };
    }
}
