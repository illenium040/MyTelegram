using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Users.Commands;
using Telegram.Application.Users.Queries;
using Telegram.Presentation.Abstractions;

namespace Telegram.Presentation.Controllers;

public sealed class UserController : ApiController
{
    public UserController(ISender sender) : base(sender) { }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByName(Guid id, CancellationToken cancellationToken)
    {
        var command = new GetUserByIdQuery(id);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByName(
        [FromQuery] string? displayName,
        [FromQuery] string? userName,
        CancellationToken cancellationToken)
    {
        if (displayName == null && userName == null)
        {
            var usersResult = await Sender.Send(new GetAllUsersQuery(), cancellationToken);
            return usersResult.IsSuccess ? Ok(usersResult.Value) : HandleFailure(usersResult);
        }

        var command = new GetUserQuery(userName, displayName);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserCommand createUserCommand,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(createUserCommand, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value!.Id);
    }
}
