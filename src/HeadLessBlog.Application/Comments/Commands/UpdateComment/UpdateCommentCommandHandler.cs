using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Domain.Entities;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, OneOf<UpdateCommentResult, UpdateCommentErrorResult>>
{
    private readonly ICommentRepository _commentRepository;

    public UpdateCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<OneOf<UpdateCommentResult, UpdateCommentErrorResult>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);

            if (comment is null || comment.IsDeleted)
            {
                return new UpdateCommentErrorResult
                {
                    Error = UpdateCommentError.CommentNotFound
                };
            }

            if (comment.UserId != request.UserId)
            {
                return new UpdateCommentErrorResult
                {
                    Error = UpdateCommentError.Unauthorized
                };
            }

            comment.Content = request.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            await _commentRepository.UpdateAsync(comment, cancellationToken);

            return new UpdateCommentResult
            {
                CommentId = comment.CommentId
            };
        }
        catch (Exception)
        {
            return new UpdateCommentErrorResult
            {
                Error = UpdateCommentError.Unknown
            };
        }
    }
}
