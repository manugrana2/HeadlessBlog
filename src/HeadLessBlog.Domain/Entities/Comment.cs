using HeadLessBlog.Domain.Common;

namespace HeadLessBlog.Domain.Entities;

public class Comment: BaseEntity
{
    public int CommentId { get; set; }
    public int PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = default!;
    public bool IsDeleted { get; set; }

    public Post? Post { get; set; }
    public User? User { get; set; }
}
