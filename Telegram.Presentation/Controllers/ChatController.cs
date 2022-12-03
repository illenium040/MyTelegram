using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Chat.Commands;
using Telegram.Application.Chat.Queries;
using Telegram.Infrastructure.Abstractions;
using Telegram.Presentation.Abstractions;

namespace Telegram.Presentation.Controllers
{
    public sealed class ChatController : ApiController
    {
        private readonly IChatRepository _chatRepository;
        public ChatController(ISender sender, IChatRepository chatRepository) : base(sender)
        {
            _chatRepository = chatRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync() => Ok(_chatRepository.GetAllAsync());

        [HttpGet("{chatId}")]
        public async Task<IActionResult> Get(
            Guid chatId,
            CancellationToken cancellationToken)
        {
            var result = await Sender.Send(new GetChatQuery(chatId), cancellationToken);
            return result.IsSuccess ? Ok(result.Value!.Id) : HandleFailure(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(
            [FromBody] CreateChatCommand createChatCommand,
            CancellationToken cancellationToken)
        {
            var result = await Sender.Send(createChatCommand, cancellationToken);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value!.Id);
        }

    }
}
