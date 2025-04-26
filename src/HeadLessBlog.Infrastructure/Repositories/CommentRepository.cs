using HeadLessBlog.Application.Comments.Queries.ListComments;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using HeadLessBlog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HeadLessBlog.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly HeadLessBlogDbContext _dbContext;

    public CommentRepository(HeadLessBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Comment> CreateAsync(Comment comment, CancellationToken cancellationToken)
    {
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return comment;
    }

    public async Task<Comment?> GetByIdAsync(int commentId, CancellationToken cancellationToken)
    {
        return await _dbContext.Comments
            .FirstOrDefaultAsync(c => c.CommentId == commentId, cancellationToken);
    }

    public async Task<Comment> UpdateAsync(Comment comment, CancellationToken cancellationToken)
    {
        _dbContext.Comments.Update(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return comment;
    }

    public async Task DeleteAsync(Comment comment, CancellationToken cancellationToken)
    {
        comment.IsDeleted = true;
        _dbContext.Comments.Update(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ListCommentsResult> ListCommentsAsync(
        Guid? userId,
        int? postId,
        DateTime? createdFrom,
        DateTime? createdTo,
        int page,
        int pageSize,
        bool isAscending,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Comments
            .AsNoTracking()
            .Where(c => !c.IsDeleted);

        if (userId.HasValue)
        {
            query = query.Where(c => c.UserId == userId.Value);
        }

        if (postId.HasValue)
        {
            query = query.Where(c => c.PostId == postId.Value);
        }

        if (createdFrom.HasValue)
        {
            query = query.Where(c => c.CreatedAt >= createdFrom.Value);
        }

        if (createdTo.HasValue)
        {
            query = query.Where(c => c.CreatedAt <= createdTo.Value);
        }

        query = isAscending
            ? query.OrderBy(c => c.CreatedAt)
            : query.OrderByDescending(c => c.CreatedAt);

        var total = await query.CountAsync(cancellationToken);

        var comments = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                PostId = c.PostId,
                UserId = c.UserId,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
            })
            .ToListAsync(cancellationToken);

        return new ListCommentsResult
        {
            Comments = comments,
            TotalCount = total
        };
    }
}
