using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Entities;
using Telegram.Domain.Primitivies;
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

        //need some tests
        public async Task<bool> IsExisting(User first, User second)
        {
            var result = await _chats
                .Include(x => x.UserChats)
                .SelectMany(x => x.UserChats.Where(uc => uc.UserId == first.Id).Select(x => x.Chat))
                .SelectMany(x => x.UserChats)
                .FirstOrDefaultAsync(x => x.UserId == second.Id);
            return result is not null;
        }
    }
}
