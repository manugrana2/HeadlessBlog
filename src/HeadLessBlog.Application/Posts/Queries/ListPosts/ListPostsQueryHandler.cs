using HeadLessBlog.Application.Common.Interfaces;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Posts.Queries.ListPosts;

public class ListPostsQueryHandler : IRequestHandler<ListPostsQuery, OneOf<ListPostsResult, ListPostsErrorResult>>
{
    private readonly IPostRepository _postRepository;

    public ListPostsQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<OneOf<ListPostsResult, ListPostsErrorResult>> Handle(ListPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _postRepository.ListPostsAsync(
                userId: request.UserId,
                title: request.Title,
                createdFrom: request.CreatedFrom,
                createdTo: request.CreatedTo,
                page: request.Page,
                pageSize: request.PageSize,
                sortBy: request.SortBy,
                isAscending: request.IsAscending,
                cancellationToken
            );

            return result;
        }
        catch (Exception)
        {
            return new ListPostsErrorResult
            {
                Error = ListPostsError.Unknown
            };
        }
    }
}
