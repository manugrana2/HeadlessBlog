namespace HeadLessBlog.WebAPI.Requests.Comments;

public class CreateCommentRequest
{
    public required int PostId { get; init; }
    public required string Content { get; init; }
}
