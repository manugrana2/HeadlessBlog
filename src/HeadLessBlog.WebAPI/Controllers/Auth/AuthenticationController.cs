using HeadLessBlog.Application.Users.Commands.LoginUser;
using HeadLessBlog.Application.Users.Commands.UpdateUser;
using HeadLessBlog.WebAPI.Extensions;
using HeadLessBlog.WebAPI.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace HeadLessBlog.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginUserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(LoginUserErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(LoginUserErrorResult), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OneOf<LoginUserResult, LoginUserErrorResult>>> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match<ActionResult>(
            success => Ok(success),
            error => error.Error == LoginUserError.InvalidCredentials
                ? Unauthorized(error)
                : BadRequest(error)
        );
    }


    [HttpPatch("me")]
    [Authorize]
    [ProducesResponseType(typeof(UpdateUserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateUserErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UpdateUserErrorResult), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OneOf<UpdateUserResult, UpdateUserErrorResult>>> UpdateMe([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var command = new UpdateUserCommand
        {
            UserId = userId,
            Name = request.Name,
            LastName = request.LastName,
            CountryCode = request.CountryCode
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match<ActionResult>(
            success => Ok(success),
            error => error.Error == UpdateUserError.Unauthorized
                ? Unauthorized(error)
                : BadRequest(error)
        );
    }
}
