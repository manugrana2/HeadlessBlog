namespace HeadLessBlog.Application.Posts.Queries.ListPosts;

public class ListPostsResult
{
    public required IEnumerable<PostSummaryDto> Posts { get; init; }
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
}
