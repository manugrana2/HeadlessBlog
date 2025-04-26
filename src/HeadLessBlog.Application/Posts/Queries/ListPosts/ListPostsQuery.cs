using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Posts.Queries.ListPosts;

public class ListPostsQuery : IRequest<OneOf<ListPostsResult, ListPostsErrorResult>>
{
    public Guid? UserId { get; init; }
    public string? Title { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public string? SortBy { get; init; }
    public bool IsAscending { get; init; } = true;
}
