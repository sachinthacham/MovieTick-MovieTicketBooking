using Moq;
using MovieBooking.Application.Features.Auth.Commands;
using MovieBooking.Application.Features.Auth.Handlers;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Tests.Auth;

public class PasswordHandlersTests
{
    [Fact]
    public async Task RequestPasswordReset_ShouldThrow_WhenEmailDoesNotExist()
    {
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.GetByEmailAsync("missing@movietick.com"))
            .ReturnsAsync((User?)null);

        var handler = new RequestPasswordResetHandler(userRepository.Object);
        var act = () => handler.Handle(new RequestPasswordResetCommand("missing@movietick.com"), CancellationToken.None);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Equal("No account found with this email address.", exception.Message);
    }

    [Fact]
    public async Task RequestPasswordReset_ShouldGenerateTokenAndPersistUser()
    {
        var userRepository = new Mock<IUserRepository>();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@movietick.com"
        };

        userRepository
            .Setup(x => x.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);
        userRepository
            .Setup(x => x.UpdateAsync(user))
            .Returns(Task.CompletedTask);

        var handler = new RequestPasswordResetHandler(userRepository.Object);
        var result = await handler.Handle(new RequestPasswordResetCommand(user.Email), CancellationToken.None);

        Assert.False(string.IsNullOrWhiteSpace(result.ResetToken));
        Assert.True(result.ResetToken.Length >= 64);
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
        Assert.Equal(result.ResetToken, user.PasswordResetToken);
        Assert.NotNull(user.PasswordResetTokenExpiry);
        userRepository.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task ChangePassword_ShouldThrow_WhenCurrentPasswordIsWrong()
    {
        var userRepository = new Mock<IUserRepository>();
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "user@movietick.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Correct@123")
        };

        userRepository
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        var handler = new ChangePasswordHandler(userRepository.Object);
        var act = () => handler.Handle(new ChangePasswordCommand(userId, "Wrong@123", "New@123456"), CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Current password is incorrect.", exception.Message);
        userRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task ChangePassword_ShouldHashAndPersistNewPassword_WhenCurrentPasswordIsValid()
    {
        var userRepository = new Mock<IUserRepository>();
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "user@movietick.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Current@123")
        };

        userRepository
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);
        userRepository
            .Setup(x => x.UpdateAsync(user))
            .Returns(Task.CompletedTask);

        var handler = new ChangePasswordHandler(userRepository.Object);
        var result = await handler.Handle(
            new ChangePasswordCommand(userId, "Current@123", "NewStrong@123"),
            CancellationToken.None);

        Assert.True(result);
        Assert.True(BCrypt.Net.BCrypt.Verify("NewStrong@123", user.PasswordHash));
        userRepository.Verify(x => x.UpdateAsync(user), Times.Once);
    }
}
