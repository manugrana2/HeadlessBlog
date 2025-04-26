using HeadLessBlog.Application.Common.Interfaces;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OneOf<LoginUserResult, LoginUserErrorResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasherService _passwordHasherService;

    public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService, IPasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<OneOf<LoginUserResult, LoginUserErrorResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null)
            {
                return new LoginUserErrorResult
                {
                    Error = LoginUserError.InvalidCredentials
                };
            }

           if (!_passwordHasherService.VerifyPassword(user.PasswordHash, request.Password))
            {
                return new LoginUserErrorResult
                {
                    Error = LoginUserError.InvalidCredentials
                };
            }

            var token = _tokenService.GenerateToken(user.UserId, user.Role.ToString());

            return new LoginUserResult
            {
                Token = token,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                CountryCode = user.CountryCode,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
        catch (Exception)
        {
            return new LoginUserErrorResult
            {
                Error = LoginUserError.Unknown
            };
        }
    }
}
