namespace HeadLessBlog.WebAPI.Models.Users;

public class UpdateUserRequest
{
    public required string Name { get; init; }
    public string? LastName { get; init; }
    public required string CountryCode { get; init; }
}
