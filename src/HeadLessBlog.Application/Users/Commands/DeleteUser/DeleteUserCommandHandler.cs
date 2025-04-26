using HeadLessBlog.Application.Common.Interfaces;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OneOf<DeleteUserResult, DeleteUserErrorResult>>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OneOf<DeleteUserResult, DeleteUserErrorResult>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null || user.IsDeleted)
            {
                return new DeleteUserErrorResult
                {
                    Error = DeleteUserError.NotFound
                };
            }

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.DeleteAsync(user, cancellationToken);

            return new DeleteUserResult
            {
                UserId = user.UserId
            };
        }
        catch (Exception)
        {
            return new DeleteUserErrorResult
            {
                Error = DeleteUserError.Unknown
            };
        }
    }
}
