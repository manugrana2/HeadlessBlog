using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommand : IRequest<OneOf<DeleteCommentResult, DeleteCommentErrorResult>>
{
    public required int CommentId { get; init; }
    public required Guid UserId { get; init; }
}
