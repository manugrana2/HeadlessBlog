namespace HeadLessBlog.Application.Posts.Commands.CreatePost;

public class CreatePostResult
{
    public required int PostId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required DateTime CreatedAt { get; init; }
}
