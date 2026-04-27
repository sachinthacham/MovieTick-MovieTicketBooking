using Moq;
using MovieBooking.Application.Features.Admin.Commands;
using MovieBooking.Application.Features.Admin.Handlers;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Tests.Admin;

public class AdminUserHandlersTests
{
    [Fact]
    public async Task UpdateUserRole_ShouldThrow_WhenRoleIsInvalid()
    {
        var userRepository = new Mock<IUserRepository>();
        var handler = new UpdateUserRoleHandler(userRepository.Object);

        var command = new UpdateUserRoleCommand(Guid.NewGuid(), "SuperAdmin");
        var act = () => handler.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Contains("Invalid role", exception.Message);
        userRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserRole_ShouldUpdateRole_WhenRoleIsValid()
    {
        var userRepository = new Mock<IUserRepository>();
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Role = "User", Email = "member@movietick.com" };

        userRepository
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);
        userRepository
            .Setup(x => x.UpdateAsync(user))
            .Returns(Task.CompletedTask);

        var handler = new UpdateUserRoleHandler(userRepository.Object);
        var result = await handler.Handle(new UpdateUserRoleCommand(userId, "Admin"), CancellationToken.None);

        Assert.True(result);
        Assert.Equal("Admin", user.Role);
        userRepository.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task DeactivateUser_ShouldSetIsActiveFalse()
    {
        var userRepository = new Mock<IUserRepository>();
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, IsActive = true, Email = "active@movietick.com" };

        userRepository
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);
        userRepository
            .Setup(x => x.UpdateAsync(user))
            .Returns(Task.CompletedTask);

        var handler = new DeactivateUserHandler(userRepository.Object);
        var result = await handler.Handle(new DeactivateUserCommand(userId), CancellationToken.None);

        Assert.True(result);
        Assert.False(user.IsActive);
        userRepository.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task ActivateUser_ShouldSetIsActiveTrue()
    {
        var userRepository = new Mock<IUserRepository>();
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, IsActive = false, Email = "inactive@movietick.com" };

        userRepository
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);
        userRepository
            .Setup(x => x.UpdateAsync(user))
            .Returns(Task.CompletedTask);

        var handler = new ActivateUserHandler(userRepository.Object);
        var result = await handler.Handle(new ActivateUserCommand(userId), CancellationToken.None);

        Assert.True(result);
        Assert.True(user.IsActive);
        userRepository.Verify(x => x.UpdateAsync(user), Times.Once);
    }
}
