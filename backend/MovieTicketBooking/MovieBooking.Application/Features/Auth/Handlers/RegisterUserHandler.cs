using MediatR;
using MovieBooking.Domain.Entities;
using MovieBooking.Application.Interfaces;
using MovieBooking.Application.Features.Auth.Commands;

namespace MovieBooking.Application.Features.Auth.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new InvalidOperationException("An account with this email already exists.");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}
