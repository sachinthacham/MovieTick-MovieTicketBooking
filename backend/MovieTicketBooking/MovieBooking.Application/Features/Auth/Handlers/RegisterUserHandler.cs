using MediatR;
using BCryptNet = BCrypt.Net.BCrypt;
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
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCryptNet.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}