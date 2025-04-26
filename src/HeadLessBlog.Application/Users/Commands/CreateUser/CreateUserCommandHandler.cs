using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using HeadLessBlog.Domain.Enums;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OneOf<CreateUserResult, CreateUserErrorResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasherService;

    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<OneOf<CreateUserResult, CreateUserErrorResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingUser is not null)
            {
                return new CreateUserErrorResult
                {
                    Error = CreateUserError.DuplicatedEmail
                };
            }

            var user = new User
            {
                Name = request.Name,
                LastName = request.LastName,
                CountryCode = request.CountryCode,
                Email = request.Email,
                PasswordHash = _passwordHasherService.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                Role = Role.Creator,
                IsDeleted = false
            };

            var createdUser = await _userRepository.CreateAsync(user, cancellationToken);

            return new CreateUserResult
            {
                UserId = createdUser.UserId
            };
        }
        catch (Exception)
        {
            return new CreateUserErrorResult
            {
                Error = CreateUserError.Unknown
            };
        }
    }

}
