using Moq;
using MovieBooking.Application.Features.Auth.Commands;
using MovieBooking.Application.Features.Auth.Handlers;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Tests.Auth;

public class RefreshTokenHandlerTests
{
    [Fact]
    public async Task Handle_ShouldThrow_WhenTokenIsInvalid()
    {
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtService>();

        refreshTokenRepository
            .Setup(x => x.GetByTokenAsync("invalid-token"))
            .ReturnsAsync((RefreshToken?)null);

        var handler = new RefreshTokenHandler(
            refreshTokenRepository.Object,
            userRepository.Object,
            jwtService.Object);

        var act = () => handler.Handle(new RefreshTokenCommand("invalid-token"), CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Invalid or expired refresh token.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserForTokenDoesNotExist()
    {
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtService>();
        var userId = Guid.NewGuid();

        refreshTokenRepository
            .Setup(x => x.GetByTokenAsync("refresh-token"))
            .ReturnsAsync(new RefreshToken
            {
                UserId = userId,
                Token = "refresh-token",
                IsRevoked = false,
                ExpiryDate = DateTime.UtcNow.AddMinutes(30)
            });
        userRepository
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        var handler = new RefreshTokenHandler(
            refreshTokenRepository.Object,
            userRepository.Object,
            jwtService.Object);

        var act = () => handler.Handle(new RefreshTokenCommand("refresh-token"), CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("User not found.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldRevokeOldTokenAndIssueNewTokens_WhenTokenIsValid()
    {
        var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        var userRepository = new Mock<IUserRepository>();
        var jwtService = new Mock<IJwtService>();
        var userId = Guid.NewGuid();
        var storedToken = new RefreshToken
        {
            UserId = userId,
            Token = "old-refresh-token",
            IsRevoked = false,
            ExpiryDate = DateTime.UtcNow.AddMinutes(30)
        };
        var user = new User
        {
            Id = userId,
            Email = "user@movietick.com",
            Role = "User"
        };

        refreshTokenRepository
            .Setup(x => x.GetByTokenAsync("old-refresh-token"))
            .ReturnsAsync(storedToken);
        userRepository
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);
        jwtService
            .Setup(x => x.GenerateToken(userId, user.Email, user.Role))
            .Returns("new-access-token");
        jwtService
            .Setup(x => x.GenerateRefreshToken())
            .Returns("new-refresh-token");

        RefreshToken? newToken = null;
        refreshTokenRepository
            .Setup(x => x.AddAsync(It.IsAny<RefreshToken>()))
            .Callback<RefreshToken>(token => newToken = token)
            .Returns(Task.CompletedTask);

        var handler = new RefreshTokenHandler(
            refreshTokenRepository.Object,
            userRepository.Object,
            jwtService.Object);

        var result = await handler.Handle(new RefreshTokenCommand("old-refresh-token"), CancellationToken.None);

        Assert.Equal("new-access-token", result.AccessToken);
        Assert.Equal("new-refresh-token", result.RefreshToken);
        Assert.True(storedToken.IsRevoked);
        Assert.NotNull(newToken);
        Assert.Equal(userId, newToken!.UserId);
        Assert.Equal("new-refresh-token", newToken.Token);
        refreshTokenRepository.Verify(x => x.UpdateAsync(storedToken), Times.Once);
        refreshTokenRepository.Verify(x => x.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
    }
}
