using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Posts.Commands.CreatePost;

public class CreatePostCommand : IRequest<OneOf<CreatePostResult, CreatePostErrorResult>>
{
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
}
