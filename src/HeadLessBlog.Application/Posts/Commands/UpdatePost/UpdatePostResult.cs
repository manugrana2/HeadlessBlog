namespace HeadLessBlog.Application.Posts.Commands.UpdatePost;

public class UpdatePostResult
{
    public required int PostId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required DateTime UpdatedAt { get; init; }
}
