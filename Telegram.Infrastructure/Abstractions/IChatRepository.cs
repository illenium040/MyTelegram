using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;

namespace Telegram.Infrastructure.Abstractions
{
    public interface IChatRepository : IRepository<Chat>
    {
        void AppendUser(UserChat user);
        Task<bool> IsExisting(User first, User second);
    }
}
