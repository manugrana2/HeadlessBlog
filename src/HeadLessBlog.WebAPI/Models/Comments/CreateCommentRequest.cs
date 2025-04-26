namespace HeadLessBlog.WebAPI.Models.Comments;

public class CreateCommentRequest
{
    public required int PostId { get; init; }
    public required string Content { get; init; }
}
