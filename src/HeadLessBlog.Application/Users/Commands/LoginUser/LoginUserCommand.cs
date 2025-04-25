using OneOf;
using MediatR;
using HeadLessBlog.Application.Users.Commands.LoginUser;

namespace HeadLessBlog.Application.Users.Commands.LoginUser;

public class LoginUserCommand : IRequest<OneOf<LoginUserResult, LoginUserErrorResult>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
