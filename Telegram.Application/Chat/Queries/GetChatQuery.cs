using Telegram.Application.Abstractions;
using ChatEntity = Telegram.Domain.Entities.Chat;

namespace Telegram.Application.Chat.Queries
{
    public sealed record GetChatQuery(Guid Id) : IQuery<ChatEntity>;
}
