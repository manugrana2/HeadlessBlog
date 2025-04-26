using FluentAssertions;
using HeadLessBlog.Application.Comments.Commands.DeleteComment;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using Moq;

namespace HeadLessBlog.UnitTests.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandlerTests
{
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly DeleteCommentCommandHandler _handler;

    public DeleteCommentCommandHandlerTests()
    {
        _commentRepositoryMock = new Mock<ICommentRepository>();
        _handler = new DeleteCommentCommandHandler(_commentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturn_Success_WhenUserOwnsComment()
    {
        var userId = Guid.NewGuid();
        var comment = new Comment { CommentId = 1, UserId = userId, Content = "Comment" };

        _commentRepositoryMock.Setup(x => x.GetByIdAsync(comment.CommentId, default))
            .ReturnsAsync(comment);

        _commentRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Comment>(), default))
            .ReturnsAsync(comment);

        var command = new DeleteCommentCommand
        {
            CommentId = comment.CommentId,
            UserId = userId
        };

        var result = await _handler.Handle(command, default);

        result.IsT0.Should().BeTrue();
        result.AsT0.CommentId.Should().Be(comment.CommentId);
    }

    [Fact]
    public async Task Handle_ShouldReturn_Unauthorized_WhenDifferentUser()
    {
        var comment = new Comment { CommentId = 1, UserId = Guid.NewGuid(), Content = "Comment" };

        _commentRepositoryMock.Setup(x => x.GetByIdAsync(comment.CommentId, default))
            .ReturnsAsync(comment);

        var command = new DeleteCommentCommand
        {
            CommentId = comment.CommentId,
            UserId = Guid.NewGuid() // Different user
        };

        var result = await _handler.Handle(command, default);

        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(DeleteCommentError.Unauthorized);
    }

    [Fact]
    public async Task Handle_ShouldReturn_NotFound_WhenCommentDoesNotExist()
    {
        _commentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync((Comment?)null);

        var command = new DeleteCommentCommand
        {
            CommentId = 1,
            UserId = Guid.NewGuid()
        };

        var result = await _handler.Handle(command, default);

        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(DeleteCommentError.CommentNotFound);
    }
}
