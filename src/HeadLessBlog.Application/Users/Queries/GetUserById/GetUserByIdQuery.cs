using HeadLessBlog.Application.Users.Queries.GetUserById;
using MediatR;
using OneOf;

namespace HeadLessBlog.Application.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<OneOf<GetUserByIdResponse, GetUserByIdErrorResult>>
{
    public Guid UserId { get; set; }
}
