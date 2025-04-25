using HeadLessBlog.Domain.Common;

namespace HeadLessBlog.Domain.Entities;

public class Post : BaseEntity
{
    public int PostId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool IsDeleted { get; set; }

    public User? User { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
