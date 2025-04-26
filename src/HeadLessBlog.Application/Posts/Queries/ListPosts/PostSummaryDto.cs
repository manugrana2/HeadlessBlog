namespace HeadLessBlog.Application.Posts.Queries.ListPosts;

public class PostSummaryDto
{
    public required int PostId { get; init; }
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}
