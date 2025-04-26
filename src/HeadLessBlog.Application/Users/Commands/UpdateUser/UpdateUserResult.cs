namespace HeadLessBlog.Application.Users.Commands.UpdateUser;

public class UpdateUserResult
{
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
    public string? LastName { get; init; }
    public required string CountryCode { get; init; }
}
