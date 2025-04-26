namespace HeadLessBlog.Application.Common.Interfaces;

using HeadLessBlog.Application.Comments.Queries.ListComments;
using HeadLessBlog.Domain.Entities;

public interface ICommentRepository
{
    Task<Comment> CreateAsync(Comment comment, CancellationToken cancellationToken);
    Task<Comment?> GetByIdAsync(int commentId, CancellationToken cancellationToken);
    Task<Comment> UpdateAsync(Comment comment, CancellationToken cancellationToken);
    Task DeleteAsync(Comment comment, CancellationToken cancellationToken);
    Task<ListCommentsResult> ListCommentsAsync(
        Guid? userId,
        int? postId,
        DateTime? createdFrom,
        DateTime? createdTo,
        int page,
        int pageSize,
        bool isAscending,
        CancellationToken cancellationToken
    );
}
