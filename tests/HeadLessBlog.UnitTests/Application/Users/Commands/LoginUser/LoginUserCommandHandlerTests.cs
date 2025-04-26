using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Application.Users.Commands.LoginUser;
using HeadLessBlog.Domain.Entities;
using HeadLessBlog.Domain.Enums;
using Moq;
using Xunit;

namespace HeadLessBlog.UnitTests.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IPasswordHasherService> _passwordHasherServiceMock;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _passwordHasherServiceMock = new Mock<IPasswordHasherService>();

        _handler = new LoginUserCommandHandler(
            _userRepositoryMock.Object,
            _tokenServiceMock.Object,
            _passwordHasherServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnLoginUserResult_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = "hashed-password",
            Role = Role.Creator,
            Name = "Test",
            CountryCode = "US",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherServiceMock
            .Setup(x => x.VerifyPassword(user.PasswordHash, It.IsAny<string>()))
            .Returns(true);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(user.UserId, user.Role.ToString()))
            .Returns("fake-jwt-token");

        var command = new LoginUserCommand
        {
            Email = user.Email,
            Password = "plaintext-password"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Token.Should().Be("fake-jwt-token");
        result.AsT0.Email.Should().Be(user.Email);
        result.AsT0.Name.Should().Be(user.Name);
        result.AsT0.Role.Should().Be(user.Role.ToString());
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidCredentials_WhenUserNotFound()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var command = new LoginUserCommand
        {
            Email = "notfound@example.com",
            Password = "any-password"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(LoginUserError.InvalidCredentials);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidCredentials_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = "hashed-password",
            Role = Role.Creator
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherServiceMock
            .Setup(x => x.VerifyPassword(user.PasswordHash, It.IsAny<string>()))
            .Returns(false);

        var command = new LoginUserCommand
        {
            Email = user.Email,
            Password = "wrong-password"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(LoginUserError.InvalidCredentials);
    }
}
