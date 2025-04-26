using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, OneOf<CreateCommentResult, CreateCommentErrorResult>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;

    public CreateCommentCommandHandler(ICommentRepository commentRepository, IPostRepository postRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
    }

    public async Task<OneOf<CreateCommentResult, CreateCommentErrorResult>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
            if (post == null || post.IsDeleted)
            {
                return new CreateCommentErrorResult
                {
                    Error = CreateCommentError.PostNotFound
                };
            }

            var comment = new Comment
            {
                PostId = request.PostId,
                UserId = request.UserId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var createdComment = await _commentRepository.CreateAsync(comment, cancellationToken);

            return new CreateCommentResult
            {
                CommentId = createdComment.CommentId
            };
        }
        catch (Exception)
        {
            return new CreateCommentErrorResult
            {
                Error = CreateCommentError.Unknown
            };
        }
    }
}
