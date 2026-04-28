using MediatR;
using MovieBooking.Application.Features.Admin.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Admin.Handlers;

public class UpdateUserRoleHandler : IRequestHandler<UpdateUserRoleCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserRoleHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var allowedRoles = new[] { "Admin", "User" };
        if (!allowedRoles.Contains(request.NewRole))
            throw new ArgumentException($"Invalid role: {request.NewRole}");

        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new KeyNotFoundException("User not found.");

        user.Role = request.NewRole;
        await _userRepository.UpdateAsync(user);
        return true;
    }
}
