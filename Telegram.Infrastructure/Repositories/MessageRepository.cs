using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Entities;
using Telegram.Domain.Enums;
using Telegram.Domain.Primitivies;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Infrastructure.Repositories;

internal sealed class MessageRepository : Repository<Message, ApplicationDbContext>, IMessageRepository
{
    private readonly DbSet<Message> _messages;
    public MessageRepository(ApplicationDbContext context) : base(context) => _messages = Context.Set<Message>();

    public void AddMessage(Message message) => _messages.Add(message);
    public IAsyncEnumerable<Message> GetMessagesByChatId(Guid chatId)
        => _messages.Where(x => x.ChatId == chatId).AsAsyncEnumerable();

    public IAsyncEnumerable<Message> GetMessagesByType(MessageType type)
        => _messages.Where(x => x.Type == type).AsAsyncEnumerable();

    public Task<int> GetMessagesCountByType(MessageType type)
        => _messages.Where(x => x.Type == type).CountAsync();
}
