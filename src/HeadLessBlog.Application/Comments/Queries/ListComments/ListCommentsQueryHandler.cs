using HeadLessBlog.Application.Common.Interfaces;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Comments.Queries.ListComments;

public class ListCommentsQueryHandler : IRequestHandler<ListCommentsQuery, OneOf<ListCommentsResult, ListCommentsErrorResult>>
{
    private readonly ICommentRepository _commentRepository;

    public ListCommentsQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<OneOf<ListCommentsResult, ListCommentsErrorResult>> Handle(ListCommentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _commentRepository.ListCommentsAsync(
                userId: request.UserId,
                postId: request.PostId,
                createdFrom: request.CreatedFrom,
                createdTo: request.CreatedTo,
                page: request.Page,
                pageSize: request.PageSize,
                isAscending: request.IsAscending,
                cancellationToken
            );

            return result;
        }
        catch (Exception)
        {
            return new ListCommentsErrorResult
            {
                Error = ListCommentsError.Unknown
            };
        }
    }
}
