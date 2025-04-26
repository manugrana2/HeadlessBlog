using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Posts.Commands.UpdatePost;

public class UpdatePostCommand : IRequest<OneOf<UpdatePostResult, UpdatePostErrorResult>>
{
    public required Guid UserId { get; init; }
    public required int PostId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
}
