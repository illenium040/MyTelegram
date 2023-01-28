using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;

namespace Telegram.Infrastructure.Abstractions;

public interface IBlockedUsersRepository : IRepository<BlockedUser>
{

}
