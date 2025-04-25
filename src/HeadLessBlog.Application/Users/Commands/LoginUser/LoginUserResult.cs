namespace HeadLessBlog.Application.Users.Commands.LoginUser;

public class LoginUserResult
{
    public required string Token { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public string? LastName { get; init; } 
    public required string Role { get; init; }
    public required string CountryCode { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}
