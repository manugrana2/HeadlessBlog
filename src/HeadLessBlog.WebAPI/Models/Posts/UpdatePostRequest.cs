namespace HeadLessBlog.WebAPI.Models.Posts;

public class UpdatePostRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }
}
