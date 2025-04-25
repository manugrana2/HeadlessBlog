namespace HeadLessBlog.Domain.Entities;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? LastName { get; set; }
    public string CountryCode { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
