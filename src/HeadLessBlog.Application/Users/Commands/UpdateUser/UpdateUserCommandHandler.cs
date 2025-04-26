using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OneOf<UpdateUserResult, UpdateUserErrorResult>>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OneOf<UpdateUserResult, UpdateUserErrorResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                return new UpdateUserErrorResult
                {
                    Error = UpdateUserError.UserNotFound
                };
            }

            user.Name = request.Name;
            user.LastName = request.LastName;
            user.CountryCode = request.CountryCode;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return new UpdateUserResult
            {
                UserId = user.UserId,
                Name = user.Name,
                LastName = user.LastName,
                CountryCode = user.CountryCode
            };
        }
        catch (Exception)
        {
            return new UpdateUserErrorResult
            {
                Error = UpdateUserError.Unknown
            };
        }
    }
}
