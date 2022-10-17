using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Infrastructure.Repositories
{
    internal class BlockedUserRepository : Repository<BlockedUser, ApplicationDbContext>, IBlockedUsersRepository
    {
        private readonly DbSet<BlockedUser> _blocked;
        public BlockedUserRepository(ApplicationDbContext context) : base(context)
        {
            _blocked = Context.Set<BlockedUser>();
        }
    }
}
