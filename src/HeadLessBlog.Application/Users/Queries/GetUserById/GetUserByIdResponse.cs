namespace HeadLessBlog.Application.Users.Queries.GetUserById;

public class GetUserByIdResponse
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public string? LastName { get; set; }
    public required string CountryCode { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}
