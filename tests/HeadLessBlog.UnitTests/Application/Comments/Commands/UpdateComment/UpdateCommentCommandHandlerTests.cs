using FluentAssertions;
using HeadLessBlog.Application.Comments.Commands.UpdateComment;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using Moq;

namespace HeadLessBlog.UnitTests.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandlerTests
{
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly UpdateCommentCommandHandler _handler;

    public UpdateCommentCommandHandlerTests()
    {
        _commentRepositoryMock = new Mock<ICommentRepository>();
        _handler = new UpdateCommentCommandHandler(_commentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturn_Success_WhenUserOwnsComment()
    {
        var userId = Guid.NewGuid();
        var comment = new Comment { CommentId = 1, UserId = userId, Content = "Old content" };

        _commentRepositoryMock.Setup(x => x.GetByIdAsync(comment.CommentId, default))
            .ReturnsAsync(comment);

        _commentRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Comment>(), default))
            .ReturnsAsync(comment);

        var command = new UpdateCommentCommand
        {
            CommentId = comment.CommentId,
            UserId = userId,
            Content = "Updated content"
        };

        var result = await _handler.Handle(command, default);

        result.IsT0.Should().BeTrue();
        result.AsT0.CommentId.Should().Be(comment.CommentId);
    }

    [Fact]
    public async Task Handle_ShouldReturn_Unauthorized_WhenDifferentUser()
    {
        var comment = new Comment { CommentId = 1, UserId = Guid.NewGuid(), Content = "Old content" };

        _commentRepositoryMock.Setup(x => x.GetByIdAsync(comment.CommentId, default))
            .ReturnsAsync(comment);

        var command = new UpdateCommentCommand
        {
            CommentId = comment.CommentId,
            UserId = Guid.NewGuid(), // Different user
            Content = "Updated content"
        };

        var result = await _handler.Handle(command, default);

        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(UpdateCommentError.Unauthorized);
    }

    [Fact]
    public async Task Handle_ShouldReturn_NotFound_WhenCommentDoesNotExist()
    {
        _commentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync((Comment?)null);

        var command = new UpdateCommentCommand
        {
            CommentId = 1,
            UserId = Guid.NewGuid(),
            Content = "Content"
        };

        var result = await _handler.Handle(command, default);

        result.IsT1.Should().BeTrue();
        result.AsT1.Error.Should().Be(UpdateCommentError.CommentNotFound);
    }
}
