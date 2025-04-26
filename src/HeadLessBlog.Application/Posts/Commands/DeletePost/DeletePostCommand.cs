using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Posts.Commands.DeletePost;

public class DeletePostCommand : IRequest<OneOf<DeletePostResult, DeletePostErrorResult>>
{
    public required Guid UserId { get; init; }
    public required int PostId { get; init; }
}
