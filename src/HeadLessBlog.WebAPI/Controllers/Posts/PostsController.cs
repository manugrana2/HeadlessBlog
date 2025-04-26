using HeadLessBlog.Application.Posts.Commands.CreatePost;
using HeadLessBlog.Application.Posts.Commands.DeletePost;
using HeadLessBlog.Application.Posts.Commands.UpdatePost;
using HeadLessBlog.WebAPI.Extensions;
using HeadLessBlog.WebAPI.Models.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace HeadLessBlog.WebAPI.Controllers.Posts;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "CreatorOnly")]
    [ProducesResponseType(typeof(CreatePostResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(CreatePostErrorResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OneOf<CreatePostResult, CreatePostErrorResult>>> Create(
        [FromBody] CreatePostRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var command = new CreatePostCommand
        {
            UserId = userId,
            Title = request.Title,
            Content = request.Content
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match<ActionResult>(
            success => Ok(success),
            error => BadRequest(error)
        );
    }

    [HttpPatch("{postId:int}")]
    [Authorize(Policy = "CreatorOnly")]
    [ProducesResponseType(typeof(UpdatePostResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(UpdatePostErrorResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OneOf<UpdatePostResult, UpdatePostErrorResult>>> Update(
    [FromRoute] int postId,
    [FromBody] UpdatePostRequest request,
    CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var command = new UpdatePostCommand
        {
            UserId = userId,
            PostId = postId,
            Title = request.Title,
            Content = request.Content
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match<ActionResult>(
            success => Ok(success),
            error => error.Error switch
            {
                UpdatePostError.Forbidden => Forbid(),
                UpdatePostError.NotFound => NotFound(),
                _ => BadRequest(error)
            }
        );
    }
    [HttpDelete("{postId:int}")]
    [Authorize(Policy = "CreatorOnly")]
    [ProducesResponseType(typeof(DeletePostResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(DeletePostErrorResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OneOf<DeletePostResult, DeletePostErrorResult>>> Delete(
    [FromRoute] int postId,
    CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var command = new DeletePostCommand
        {
            UserId = userId,
            PostId = postId
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match<ActionResult>(
            success => Ok(success),
            error => error.Error switch
            {
                DeletePostError.Forbidden => Forbid(),
                DeletePostError.NotFound => NotFound(),
                _ => BadRequest(error)
            }
        );
    }

}
