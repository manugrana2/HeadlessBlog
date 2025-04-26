namespace HeadLessBlog.WebAPI.Models.Comments;

public class ListCommentsRequest
{
    public Guid? UserId { get; init; }
    public int? PostId { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public bool IsAscending { get; init; } = true;
}
