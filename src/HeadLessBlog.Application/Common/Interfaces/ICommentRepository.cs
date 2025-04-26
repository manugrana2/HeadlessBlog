namespace HeadLessBlog.Application.Common.Interfaces;

using HeadLessBlog.Domain.Entities;

public interface ICommentRepository
{
    Task<Comment> CreateAsync(Comment comment, CancellationToken cancellationToken);
    Task<Comment?> GetByIdAsync(int commentId, CancellationToken cancellationToken);
    Task<Comment> UpdateAsync(Comment comment, CancellationToken cancellationToken);
    Task DeleteAsync(Comment comment, CancellationToken cancellationToken);
    IQueryable<Comment> Query();
}
