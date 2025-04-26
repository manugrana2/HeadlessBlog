using HeadLessBlog.Application.Common.Interfaces;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Posts.Commands.DeletePost;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, OneOf<DeletePostResult, DeletePostErrorResult>>
{
    private readonly IPostRepository _postRepository;

    public DeletePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<OneOf<DeletePostResult, DeletePostErrorResult>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingPost = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);

            if (existingPost is null || existingPost.IsDeleted)
            {
                return new DeletePostErrorResult
                {
                    Error = DeletePostError.NotFound
                };
            }

            if (existingPost.UserId != request.UserId)
            {
                return new DeletePostErrorResult
                {
                    Error = DeletePostError.Forbidden
                };
            }

            existingPost.IsDeleted = true;
            existingPost.UpdatedAt = DateTime.UtcNow;

            await _postRepository.DeleteAsync(existingPost, cancellationToken);

            return new DeletePostResult
            {
                PostId = existingPost.PostId
            };
        }
        catch (Exception)
        {
            return new DeletePostErrorResult
            {
                Error = DeletePostError.Unknown
            };
        }
    }
}
