using HeadLessBlog.Application.Comments.Commands.CreateComment;
using HeadLessBlog.Application.Comments.Commands.DeleteComment;
using HeadLessBlog.Application.Comments.Commands.UpdateComment;
using HeadLessBlog.Application.Comments.Queries.ListComments;
using HeadLessBlog.WebAPI.Models.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HeadLessBlog.WebAPI.Controllers.Comments;

[ApiController]
[Route("api/comments")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCommentResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateCommentRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateCommentCommand
        {
            PostId = request.PostId,
            Content = request.Content,
            UserId = userId
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            success => CreatedAtAction(nameof(Create), new { id = success.CommentId }, success),
            error => error.Error switch
            {
                CreateCommentError.PostNotFound => NotFound("The post was not found."),
                _ => Problem()
            }
        );
    }
    [HttpPatch("{commentId:int}")]
    [ProducesResponseType(typeof(UpdateCommentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int commentId, [FromBody] UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new UpdateCommentCommand
        {
            CommentId = commentId,
            Content = request.Content,
            UserId = userId
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            success => Ok(success),
            error => error.Error switch
            {
                UpdateCommentError.CommentNotFound => NotFound("The comment was not found."),
                UpdateCommentError.Unauthorized => Forbid(),
                _ => Problem()
            }
        );
    }

    [HttpDelete("{commentId:int}")]
    [ProducesResponseType(typeof(DeleteCommentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int commentId, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new DeleteCommentCommand
        {
            CommentId = commentId,
            UserId = userId
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            success => Ok(success),
            error => error.Error switch
            {
                DeleteCommentError.CommentNotFound => NotFound("The comment was not found."),
                DeleteCommentError.Unauthorized => Forbid(),
                _ => Problem()
            }
        );
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListCommentsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListComments([FromQuery] ListCommentsRequest request, CancellationToken cancellationToken)
    {
        var query = new ListCommentsQuery
        {
            UserId = request.UserId,
            PostId = request.PostId,
            CreatedFrom = request.CreatedFrom,
            CreatedTo = request.CreatedTo,
            Page = request.Page,
            PageSize = request.PageSize,
            IsAscending = request.IsAscending
        };

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            success => Ok(success),
            error => Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred while listing comments."
            )
        );
    }


}
