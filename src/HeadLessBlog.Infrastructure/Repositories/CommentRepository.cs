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
        _dbContext.Comments.Update(comment); // Soft delete puede hacerse con flag IsDeleted
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<Comment> Query()
    {
        return _dbContext.Comments.AsQueryable();
    }
}
