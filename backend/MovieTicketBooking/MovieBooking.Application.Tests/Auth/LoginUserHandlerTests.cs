using Microsoft.Extensions.Logging;
using Moq;
using MovieBooking.Application.Features.Auth.Commands;
using MovieBooking.Application.Features.Auth.Handlers;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Tests.Auth;

public class LoginUserHandlerTests
{
    [Fact]
    public async Task Handle_ShouldThrow_WhenUserDoesNotExist()
    {
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtService>();
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var logger = new Mock<ILogger<LoginUserHandler>>();

        userRepository
            .Setup(x => x.GetByEmailAsync("missing@movietick.com"))
            .ReturnsAsync((User?)null);

        var handler = new LoginUserHandler(
            userRepository.Object,
            jwtService.Object,
            refreshTokenRepository.Object,
            logger.Object);

        var command = new LoginUserCommand("missing@movietick.com", "Password@123");
        var act = () => handler.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Invalid credentials", exception.Message);
        refreshTokenRepository.Verify(x => x.AddAsync(It.IsAny<RefreshToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenPasswordIsInvalid()
    {
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtService>();
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var logger = new Mock<ILogger<LoginUserHandler>>();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@movietick.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Correct@123"),
            Role = "User"
        };

        userRepository
            .Setup(x => x.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);

        var handler = new LoginUserHandler(
            userRepository.Object,
            jwtService.Object,
            refreshTokenRepository.Object,
            logger.Object);

        var command = new LoginUserCommand(user.Email, "Wrong@123");
        var act = () => handler.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Invalid credentials", exception.Message);
        refreshTokenRepository.Verify(x => x.AddAsync(It.IsAny<RefreshToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnTokensAndPersistRefreshToken_WhenCredentialsAreValid()
    {
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtService>();
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var logger = new Mock<ILogger<LoginUserHandler>>();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@movietick.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123456"),
            Role = "Admin"
        };

        userRepository
            .Setup(x => x.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);

        jwtService
            .Setup(x => x.GenerateToken(user.Id, user.Email, user.Role))
            .Returns("access-token");
        jwtService
            .Setup(x => x.GenerateRefreshToken())
            .Returns("refresh-token");

        RefreshToken? persistedToken = null;
        refreshTokenRepository
            .Setup(x => x.AddAsync(It.IsAny<RefreshToken>()))
            .Callback<RefreshToken>(token => persistedToken = token)
            .Returns(Task.CompletedTask);

        var handler = new LoginUserHandler(
            userRepository.Object,
            jwtService.Object,
            refreshTokenRepository.Object,
            logger.Object);

        var result = await handler.Handle(
            new LoginUserCommand(user.Email, "Admin@123456"),
            CancellationToken.None);

        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.NotNull(persistedToken);
        Assert.Equal(user.Id, persistedToken!.UserId);
        Assert.Equal("refresh-token", persistedToken.Token);
        refreshTokenRepository.Verify(x => x.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
    }
}
