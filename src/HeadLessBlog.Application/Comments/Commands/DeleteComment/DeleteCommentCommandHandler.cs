using HeadLessBlog.Application.Common.Interfaces;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, OneOf<DeleteCommentResult, DeleteCommentErrorResult>>
{
    private readonly ICommentRepository _commentRepository;

    public DeleteCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<OneOf<DeleteCommentResult, DeleteCommentErrorResult>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);

            if (comment is null || comment.IsDeleted)
            {
                return new DeleteCommentErrorResult
                {
                    Error = DeleteCommentError.CommentNotFound
                };
            }

            if (comment.UserId != request.UserId)
            {
                return new DeleteCommentErrorResult
                {
                    Error = DeleteCommentError.Unauthorized
                };
            }

            comment.IsDeleted = true;
            comment.UpdatedAt = DateTime.UtcNow;

            await _commentRepository.UpdateAsync(comment, cancellationToken);

            return new DeleteCommentResult
            {
                CommentId = comment.CommentId
            };
        }
        catch (Exception)
        {
            return new DeleteCommentErrorResult
            {
                Error = DeleteCommentError.Unknown
            };
        }
    }
}
