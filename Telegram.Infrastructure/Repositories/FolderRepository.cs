using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Infrastructure.Repositories
{
    internal class FolderRepository : Repository<Folder, ApplicationDbContext>, IFolderRepository
    {
        private readonly DbSet<Folder> _folders;
        public FolderRepository(ApplicationDbContext context) : base(context)
        {
            _folders = Context.Set<Folder>();
        }
    }
}
