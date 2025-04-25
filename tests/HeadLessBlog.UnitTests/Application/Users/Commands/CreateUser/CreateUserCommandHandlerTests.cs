using FluentAssertions;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Application.Users.Commands.CreateUser;
using Moq;
using Xunit;

namespace HeadLessBlog.UnitTests.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new CreateUserCommandHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_Create_User_Successfully()
    {
        // Arrange
        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((HeadLessBlog.Domain.Entities.User?)null);

        _userRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<HeadLessBlog.Domain.Entities.User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((HeadLessBlog.Domain.Entities.User user, CancellationToken _) => user);

        var command = new CreateUserCommand
        {
            Username = "janedoe",
            Name = "Jane",
            LastName = "Doe",
            CountryCode = "US",
            Email = "jane@example.com",
            Password = "password123"
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT0.Should().BeTrue();
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<HeadLessBlog.Domain.Entities.User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_DuplicatedEmail_Error_When_Email_Already_Exists()
    {
        // Arrange
        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HeadLessBlog.Domain.Entities.User());

        var command = new CreateUserCommand
        {
            Username = "johnsmith",
            Name = "John",
            LastName = "Smith",
            CountryCode = "US",
            Email = "duplicate@example.com",
            Password = "password"
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(CreateUserError.DuplicatedEmail);
        _userRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<HeadLessBlog.Domain.Entities.User>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
