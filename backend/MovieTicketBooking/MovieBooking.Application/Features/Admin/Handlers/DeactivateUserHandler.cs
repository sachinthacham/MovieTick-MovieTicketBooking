using MediatR;
using MovieBooking.Application.Features.Admin.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Admin.Handlers;

public class DeactivateUserHandler : IRequestHandler<DeactivateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeactivateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new KeyNotFoundException("User not found.");

        user.IsActive = false;
        await _userRepository.UpdateAsync(user);
        return true;
    }
}

public class ActivateUserHandler : IRequestHandler<ActivateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ActivateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new KeyNotFoundException("User not found.");

        user.IsActive = true;
        await _userRepository.UpdateAsync(user);
        return true;
    }
}
