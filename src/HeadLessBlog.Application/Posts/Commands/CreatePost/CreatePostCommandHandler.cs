using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Posts.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, OneOf<CreatePostResult, CreatePostErrorResult>>
{
    private readonly IPostRepository _postRepository;

    public CreatePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<OneOf<CreatePostResult, CreatePostErrorResult>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var post = new Post
            {
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var createdPost = await _postRepository.CreateAsync(post, cancellationToken);

            return new CreatePostResult
            {
                PostId = createdPost.PostId,
                Title = createdPost.Title,
                Content = createdPost.Content,
                CreatedAt = createdPost.CreatedAt
            };
        }
        catch (Exception)
        {
            return new CreatePostErrorResult
            {
                Error = CreatePostError.Unknown
            };
        }
    }
}
