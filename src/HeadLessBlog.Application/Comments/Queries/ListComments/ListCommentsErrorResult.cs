namespace HeadLessBlog.Application.Comments.Queries.ListComments;

public class ListCommentsErrorResult
{
    public required ListCommentsError Error { get; init; }
}

public enum ListCommentsError
{
    Unknown = 0
}
