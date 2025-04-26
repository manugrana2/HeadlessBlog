using FluentAssertions;
using HeadLessBlog.Application.Comments.Commands.CreateComment;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using Moq;

namespace HeadLessBlog.UnitTests.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandHandlerTests
{
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly CreateCommentCommandHandler _handler;

    public CreateCommentCommandHandlerTests()
    {
        _commentRepositoryMock = new Mock<ICommentRepository>();
        _postRepositoryMock = new Mock<IPostRepository>();
        _handler = new CreateCommentCommandHandler(_commentRepositoryMock.Object, _postRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturn_Success_WhenPostExists()
    {
        var post = new Post { PostId = 1, UserId = Guid.NewGuid(), Title = "Test Post", CreatedAt = DateTime.UtcNow };
        var comment = new Comment { CommentId = 123 };

        _postRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(post);

        _commentRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Comment>(), default))
            .ReturnsAsync(comment);

        var command = new CreateCommentCommand
        {
            PostId = 1,
            UserId = Guid.NewGuid(),
            Content = "This is a test comment."
        };

        var result = await _handler.Handle(command, default);

        result.IsT0.Should().BeTrue();
        result.AsT0.CommentId.Should().Be(comment.CommentId);
    }

    [Fact]
    public async Task Handle_ShouldReturn_PostNotFound_WhenPostDoesNotExist()
    {
        _postRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync((Post?)null);

        var command = new CreateCommentCommand
        {
            PostId = 1,
            UserId = Guid.NewGuid(),
            Content = "This is a test comment."
        };

        var result = await _handler.Handle(command, default);

        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(CreateCommentError.PostNotFound);
    }
}
