using Telegram.Application.Abstractions;
using Telegram.Domain.Enums;
using ChatEntity = Telegram.Domain.Entities.Chat;

namespace Telegram.Application.Chat.Commands
{
    public sealed record CreateChatCommand(
        Guid InitiatorUser,
        Guid ReceiverUser,
        ChatType ChatType = ChatType.Private)
        : ICommand<ChatEntity>;
}
