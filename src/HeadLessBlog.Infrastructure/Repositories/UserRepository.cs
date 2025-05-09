using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using HeadLessBlog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HeadLessBlog.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HeadLessBlogDbContext _dbContext;

    public UserRepository(HeadLessBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted, cancellationToken);
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }
    public async Task DeleteAsync(User user, CancellationToken cancellationToken)
{
    _dbContext.Users.Update(user); 
    await _dbContext.SaveChangesAsync(cancellationToken);
}
}
