using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Users.Commands;
using Telegram.Application.Users.Queries;
using Telegram.Infrastructure.Abstractions;
using Telegram.Presentation.Abstractions;

namespace Telegram.Presentation.Controllers
{
    public sealed class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;
        public UserController(ISender sender, IUserRepository userRepository) : base(sender)
        {
            _userRepository = userRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync() => Ok(await _userRepository.GetAllAsync().ToListAsync());

        [HttpGet]
        public async Task<IActionResult> GetByName(
            [FromQuery] string? displayName,
            [FromQuery] string? userName,
            [FromQuery] Guid? id,
            CancellationToken cancellationToken)
        {
            var command = new GetUserQuery(id, userName, displayName);
            var result = await Sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value!.Id) : HandleFailure(result);
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
}
