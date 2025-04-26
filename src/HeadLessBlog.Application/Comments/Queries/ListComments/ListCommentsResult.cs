namespace HeadLessBlog.Application.Comments.Queries.ListComments;

public class ListCommentsResult
{
    public required IEnumerable<CommentDto> Comments { get; init; }
    public required int TotalCount { get; init; }
}

public class CommentDto
{
    public required int CommentId { get; init; }
    public required int PostId { get; init; }
    public required Guid UserId { get; init; }
    public required string Content { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}
