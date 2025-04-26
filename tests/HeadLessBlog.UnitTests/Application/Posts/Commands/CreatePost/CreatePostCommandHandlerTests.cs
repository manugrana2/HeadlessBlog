using FluentAssertions;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Application.Posts.Commands.CreatePost;
using HeadLessBlog.Domain.Entities;
using Moq;

namespace HeadLessBlog.UnitTests.Application.Posts.Commands.CreatePost;

public class CreatePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly CreatePostCommandHandler _handler;

    public CreatePostCommandHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _handler = new CreatePostCommandHandler(_postRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Post_Is_Created()
    {
        // Arrange
        var command = new CreatePostCommand
        {
            UserId = Guid.NewGuid(),
            Title = "My Post Title",
            Content = "This is a great content."
        };

        _postRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Post post, CancellationToken _) => 
            {
                post.PostId = 1; // Simulate DB-generated ID
                return post;
            });

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT0.Should().BeTrue(); // T0 = Success case
        result.AsT0.Title.Should().Be(command.Title);
        result.AsT0.Content.Should().Be(command.Content);
        result.AsT0.PostId.Should().Be(1);
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_Exception_Occurs()
    {
        // Arrange
        var command = new CreatePostCommand
        {
            UserId = Guid.NewGuid(),
            Title = "Title",
            Content = "Content"
        };

        _postRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsT1.Should().BeTrue(); // T1 = Error case
        result.AsT1.Error.Should().Be(CreatePostError.Unknown);
    }
}
