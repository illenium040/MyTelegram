using Telegram.Domain.Shared;

namespace Telegram.Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        Task<Result> SaveChangesAsync();
        Result SaveChanges();
    }
}
