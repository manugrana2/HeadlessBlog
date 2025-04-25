using HeadLessBlog.Application.Common.Interfaces;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, OneOf<GetUserByIdResponse, GetUserByIdErrorResult>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OneOf<GetUserByIdResponse, GetUserByIdErrorResult>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null || user.IsDeleted)
            {
                return new GetUserByIdErrorResult { Error = GetUserByIdError.UserNotFound };
            }

            return new GetUserByIdResponse
            {
                UserId = user.UserId,
                Name = user.Name,
                LastName = user.LastName,
                CountryCode = user.CountryCode,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
        catch (Exception)
        {
            return new GetUserByIdErrorResult { Error = GetUserByIdError.Unknown };
        }
    }
}
