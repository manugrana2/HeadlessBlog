using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<OneOf<CreateUserResult, CreateUserErrorResult>>
{
    public required string Name { get; init; }
    public string? LastName { get; init; }
    public required string CountryCode { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
