namespace HeadLessBlog.Application.Common.Interfaces;

using HeadLessBlog.Application.Posts.Queries.ListPosts;
using HeadLessBlog.Domain.Entities;

public interface IPostRepository
{
    Task<Post> CreateAsync(Post post, CancellationToken cancellationToken);
    Task<Post?> GetByIdAsync(int postId, CancellationToken cancellationToken);
    Task<Post> UpdateAsync(Post post, CancellationToken cancellationToken);
    Task DeleteAsync(Post post, CancellationToken cancellationToken);
    Task<ListPostsResult> ListPostsAsync(
    Guid? userId,
    string? title,
    DateTime? createdFrom,
    DateTime? createdTo,
    int page,
    int pageSize,
    string? sortBy,
    bool isAscending,
    CancellationToken cancellationToken);



}
