using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<OneOf<DeleteUserResult, DeleteUserErrorResult>>
{
    public required Guid UserId { get; init; }
}
