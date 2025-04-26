using HeadLessBlog.Application.Users.Commands.CreateUser;
using HeadLessBlog.Application.Users.Commands.DeleteUser;
using HeadLessBlog.Application.Users.Queries.GetUserById;
using HeadLessBlog.WebAPI.Extensions;
using HeadLessBlog.WebAPI.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace HeadLessBlog.WebAPI.Controllers.Auth;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateUserErrorResult), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand
        {
            Name = request.Name,
            LastName = request.LastName,
            CountryCode = request.CountryCode,
            Email = request.Email,
            Password = request.Password
        };

        var result = await _mediator.Send(command);

        return result.Match<IActionResult>(
            success => Ok(success),
            error => error.Error == CreateUserError.DuplicatedEmail
                ? Conflict(error)
                : StatusCode(StatusCodes.Status500InternalServerError, error)
        );
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(GetUserByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            success => Ok(success),
            error => error.Error switch
            {
                GetUserByIdError.UserNotFound => NotFound(),
                _ => Problem(title: "An unknown error occurred.", statusCode: StatusCodes.Status500InternalServerError)
            }
        );
    }

    [HttpDelete("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var command = new DeleteUserCommand
        {
            UserId = userId
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            success => NoContent(),
            error => error.Error switch
            {
                DeleteUserError.NotFound => NotFound(),
                _ => Problem()
            }
        );
    }
}
