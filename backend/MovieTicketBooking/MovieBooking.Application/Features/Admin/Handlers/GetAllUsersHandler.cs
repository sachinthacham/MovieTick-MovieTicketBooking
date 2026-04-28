using MediatR;
using MovieBooking.Application.DTOs.Admin;
using MovieBooking.Application.Features.Admin.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Admin.Handlers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, PagedUsersDto>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedUsersDto> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _userRepository.GetAllAsync(request.Page, request.PageSize, request.Search, request.Role);

        return new PagedUsersDto
        {
            Items = users.Select(u => new UserAdminDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                PhoneNumber = u.PhoneNumber,
                ProfileImageUrl = u.ProfileImageUrl,
                IsActive = u.IsActive,
                SocialProvider = u.SocialProvider,
                CreatedAt = u.CreatedAt,
                TotalBookings = u.Bookings?.Count ?? 0,
                TotalSpent = u.Bookings?.Sum(b => b.TotalAmount) ?? 0
            }).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
