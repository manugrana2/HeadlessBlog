using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using HeadLessBlog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HeadLessBlog.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly HeadLessBlogDbContext _dbContext;

    public PostRepository(HeadLessBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Post?> GetByIdAsync(int postId, CancellationToken cancellationToken)
    {
        return await _dbContext.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PostId == postId, cancellationToken);
    }

    public async Task<Post> CreateAsync(Post post, CancellationToken cancellationToken)
    {
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return post;
    }

    public async Task<Post> UpdateAsync(Post post, CancellationToken cancellationToken)
    {
        _dbContext.Posts.Update(post);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return post;
    }
    public async Task DeleteAsync(Post post, CancellationToken cancellationToken)
    {
        _dbContext.Posts.Update(post);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
