using HeadLessBlog.Application.Users.Commands.CreateUser;
using HeadLessBlog.WebAPI.Models.Users;
using MediatR;
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
            Username = request.Username,
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
}
