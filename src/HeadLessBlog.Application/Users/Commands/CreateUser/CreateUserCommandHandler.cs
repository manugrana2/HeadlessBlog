using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OneOf<CreateUserResult, CreateUserErrorResult>>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
                Username = request.Username,
                Name = request.Name,
                LastName = request.LastName,
                CountryCode = request.CountryCode,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
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

    private string HashPassword(string password)
    {
        // TO DO HASH PASSWORD
        return $"hashed_{password}";
    }
}
