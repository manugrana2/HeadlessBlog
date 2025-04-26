using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Application.Posts.Queries.ListPosts;
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
    public async Task<ListPostsResult> ListPostsAsync(
    Guid? userId,
    string? title,
    DateTime? createdFrom,
    DateTime? createdTo,
    int page,
    int pageSize,
    string? sortBy,
    bool isAscending,
    CancellationToken cancellationToken)
    {
        var query = _dbContext.Posts.AsNoTracking().Where(p => !p.IsDeleted);

        if (userId.HasValue)
            query = query.Where(p => p.UserId == userId.Value);

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(p => p.Title.Contains(title));

        if (createdFrom.HasValue)
            query = query.Where(p => p.CreatedAt >= createdFrom.Value);

        if (createdTo.HasValue)
            query = query.Where(p => p.CreatedAt <= createdTo.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "title" => isAscending ? query.OrderBy(p => p.Title) : query.OrderByDescending(p => p.Title),
                "createdat" => isAscending ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt) // Default sort
            };
        }
        else
        {
            query = query.OrderByDescending(p => p.CreatedAt); // Default sort
        }

        var posts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PostSummaryDto
            {
                PostId = p.PostId,
                UserId = p.UserId,
                Title = p.Title,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
            })
            .ToListAsync(cancellationToken);

        return new ListPostsResult
        {
            Posts = posts,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

}
