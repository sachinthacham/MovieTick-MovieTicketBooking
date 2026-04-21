using MediatR;
using MovieBooking.Application.Features.Auth.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Auth.Handlers;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ResetPasswordHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null
            || user.PasswordResetToken != request.Token
            || user.PasswordResetTokenExpiry == null
            || user.PasswordResetTokenExpiry < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid or expired password reset token.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return true;
    }
}
