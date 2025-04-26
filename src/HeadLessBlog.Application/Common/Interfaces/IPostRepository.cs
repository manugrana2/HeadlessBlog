namespace HeadLessBlog.Application.Common.Interfaces;

using HeadLessBlog.Domain.Entities;

public interface IPostRepository
{
    Task<Post> CreateAsync(Post post, CancellationToken cancellationToken);
    Task<Post?> GetByIdAsync(int postId, CancellationToken cancellationToken);
    Task<Post> UpdateAsync(Post post, CancellationToken cancellationToken);
    Task DeleteAsync(Post post, CancellationToken cancellationToken);


}
