namespace HeadLessBlog.WebAPI.Models.Users;

public class CreateUserRequest
{
    public required string Username { get; set; }
    public required string Name { get; set; }
    public string? LastName { get; set; }
    public required string CountryCode { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
