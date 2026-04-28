using MediatR;
using MovieBooking.Application.DTOs.Auth;
using MovieBooking.Application.Features.Auth.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Auth.Handlers;

public class RequestPasswordResetHandler : IRequestHandler<RequestPasswordResetCommand, PasswordResetResponseDto>
{
    private readonly IUserRepository _userRepository;

    public RequestPasswordResetHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PasswordResetResponseDto> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new KeyNotFoundException("No account found with this email address.");

        var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        var expiry = DateTime.UtcNow.AddHours(1);

        user.PasswordResetToken = token;
        user.PasswordResetTokenExpiry = expiry;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return new PasswordResetResponseDto
        {
            ResetToken = token,
            ExpiresAt = expiry
        };
    }
}
