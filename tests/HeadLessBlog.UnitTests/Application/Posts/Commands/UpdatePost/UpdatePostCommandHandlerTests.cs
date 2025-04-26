using FluentAssertions;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Application.Posts.Commands.UpdatePost;
using HeadLessBlog.Domain.Entities;
using Moq;

namespace HeadLessBlog.UnitTests.Application.Posts.Commands.UpdatePost;

public class UpdatePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly UpdatePostCommandHandler _handler;

    public UpdatePostCommandHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _handler = new UpdatePostCommandHandler(_postRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Update_Is_Valid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingPost = new Post
        {
            PostId = 1,
            UserId = userId,
            Title = "Old Title",
            Content = "Old Content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _postRepositoryMock.Setup(x => x.GetByIdAsync(existingPost.PostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPost);

        _postRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Post post, CancellationToken _) => post);

        var command = new UpdatePostCommand
        {
            UserId = userId,
            PostId = existingPost.PostId,
            Title = "Updated Title",
            Content = "Updated Content"
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT0.Should().BeTrue(); // Success
        result.AsT0.Title.Should().Be("Updated Title");
        result.AsT0.Content.Should().Be("Updated Content");
    }

    [Fact]
    public async Task Handle_Should_Return_NotFound_When_Post_Does_Not_Exist()
    {
        // Arrange
        _postRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Post?)null);

        var command = new UpdatePostCommand
        {
            UserId = Guid.NewGuid(),
            PostId = 999,
            Title = "Any",
            Content = "Any"
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(UpdatePostError.NotFound);
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
            UpdatedAt = DateTime.UtcNow
        };

        _postRepositoryMock.Setup(x => x.GetByIdAsync(existingPost.PostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPost);

        var command = new UpdatePostCommand
        {
            UserId = Guid.NewGuid(), // Different user
            PostId = existingPost.PostId,
            Title = "New Title",
            Content = "New Content"
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(UpdatePostError.Forbidden);
    }

    [Fact]
    public async Task Handle_Should_Return_Unknown_When_Exception_Occurs()
    {
        // Arrange
        _postRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        var command = new UpdatePostCommand
        {
            UserId = Guid.NewGuid(),
            PostId = 1,
            Title = "Any",
            Content = "Any"
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(UpdatePostError.Unknown);
    }
}
