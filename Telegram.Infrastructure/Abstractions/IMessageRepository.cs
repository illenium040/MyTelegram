using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Enums;

namespace Telegram.Infrastructure.Abstractions
{
    public interface IMessageRepository : IRepository<Message>
    {
        IAsyncEnumerable<Message> GetMessagesByChatId(Guid chatId);
        IAsyncEnumerable<Message> GetMessagesByType(MessageType type);
        Task<int> GetMessagesCountByType(MessageType type);
    }
}
