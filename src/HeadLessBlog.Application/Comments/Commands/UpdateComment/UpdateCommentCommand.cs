using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommand : IRequest<OneOf<UpdateCommentResult, UpdateCommentErrorResult>>
{
    public required int CommentId { get; init; }
    public required Guid UserId { get; init; } // Verificación de Ownership
    public required string Content { get; init; }
}
