using FluentAssertions;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Application.Posts.Commands.DeletePost;
using HeadLessBlog.Domain.Entities;
using Moq;

namespace HeadLessBlog.UnitTests.Application.Posts.Commands.DeletePost;

public class DeletePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly DeletePostCommandHandler _handler;

    public DeletePostCommandHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _handler = new DeletePostCommandHandler(_postRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Delete_Is_Valid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingPost = new Post
        {
            PostId = 1,
            UserId = userId,
            Title = "Title",
            Content = "Content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _postRepositoryMock.Setup(x => x.GetByIdAsync(existingPost.PostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPost);

        _postRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new DeletePostCommand
        {
            UserId = userId,
            PostId = existingPost.PostId
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.PostId.Should().Be(existingPost.PostId);
    }

    [Fact]
    public async Task Handle_Should_Return_NotFound_When_Post_Does_Not_Exist()
    {
        // Arrange
        _postRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Post?)null);

        var command = new DeletePostCommand
        {
            UserId = Guid.NewGuid(),
            PostId = 999
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(DeletePostError.NotFound);
    }

    [Fact]
    public async Task Handle_Should_Return_Forbidden_When_User_Is_Not_Owner()
    {
        // Arrange
        var existingPost = new Post
        {
            PostId = 1,
            UserId = Guid.NewGuid(), // Different user
            Title = "Title",
            Content = "Content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _postRepositoryMock.Setup(x => x.GetByIdAsync(existingPost.PostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPost);

        var command = new DeletePostCommand
        {
            UserId = Guid.NewGuid(), // Different user
            PostId = existingPost.PostId
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(DeletePostError.Forbidden);
    }

    [Fact]
    public async Task Handle_Should_Return_Unknown_When_Exception_Occurs()
    {
        // Arrange
        _postRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        var command = new DeletePostCommand
        {
            UserId = Guid.NewGuid(),
            PostId = 1
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(DeletePostError.Unknown);
    }
}
