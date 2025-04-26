
namespace HeadLessBlog.WebAPI.Models.Posts;

public class ListPostsRequest
{
    public Guid? UserId { get; init; }
    public string? Title { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public PostSortBy SortBy { get; init; } = PostSortBy.CreatedAt; 
    public bool IsAscending { get; init; } = true;
}
