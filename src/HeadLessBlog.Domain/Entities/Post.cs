namespace HeadLessBlog.Domain.Entities;

public class Post
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public User? User { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
