using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Infrastructure.Repositories
{
    internal class ChatRepository : Repository<Chat, ApplicationDbContext>, IChatRepository
    {
        private readonly DbSet<Chat> _chats;
        private readonly DbSet<UserChat> _userChats;
        public ChatRepository(ApplicationDbContext context) : base(context)
        {
            _chats = Context.Set<Chat>();
            _userChats = Context.Set<UserChat>();
        }

        public void AppendUser(UserChat user) => _userChats.Add(user);
    }
}
