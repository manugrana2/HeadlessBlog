namespace HeadLessBlog.Application.Common.Interfaces;

using HeadLessBlog.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<User> CreateAsync(User user, CancellationToken cancellationToken);
    Task<User> UpdateAsync(User user, CancellationToken cancellationToken); 
    Task DeleteAsync(User user, CancellationToken cancellationToken);

}
