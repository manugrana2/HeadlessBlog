using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Comments.Commands.CreateComment;

public class CreateCommentCommand : IRequest<OneOf<CreateCommentResult, CreateCommentErrorResult>>
{
    public required int PostId { get; init; }
    public required Guid UserId { get; init; }
    public required string Content { get; init; }
}
