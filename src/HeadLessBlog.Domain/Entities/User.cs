using HeadLessBlog.Domain.Enums;

namespace HeadLessBlog.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = default!;
    public string? LastName { get; set; }
    public string CountryCode { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public Role Role { get; set; } = Role.Creator;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
