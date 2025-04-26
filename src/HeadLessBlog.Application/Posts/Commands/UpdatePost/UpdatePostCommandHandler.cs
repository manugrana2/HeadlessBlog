using HeadLessBlog.Application.Common.Interfaces;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Posts.Commands.UpdatePost;

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, OneOf<UpdatePostResult, UpdatePostErrorResult>>
{
    private readonly IPostRepository _postRepository;

    public UpdatePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<OneOf<UpdatePostResult, UpdatePostErrorResult>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingPost = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);

            if (existingPost is null || existingPost.IsDeleted)
            {
                return new UpdatePostErrorResult
                {
                    Error = UpdatePostError.NotFound
                };
            }

            if (existingPost.UserId != request.UserId)
            {
                return new UpdatePostErrorResult
                {
                    Error = UpdatePostError.Forbidden
                };
            }

            existingPost.Title = request.Title;
            existingPost.Content = request.Content;
            existingPost.UpdatedAt = DateTime.UtcNow;

            var updatedPost = await _postRepository.UpdateAsync(existingPost, cancellationToken);

            return new UpdatePostResult
            {
                PostId = updatedPost.PostId,
                Title = updatedPost.Title,
                Content = updatedPost.Content,
                UpdatedAt = updatedPost.UpdatedAt
            };
        }
        catch (Exception)
        {
            return new UpdatePostErrorResult
            {
                Error = UpdatePostError.Unknown
            };
        }
    }
}
