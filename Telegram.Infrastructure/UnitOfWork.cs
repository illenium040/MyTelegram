using Telegram.Domain.Abstractions;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Errors;

namespace Telegram.Infrastructure;

internal class UnitOfWork : IUnitOfWork<ApplicationDbContext>, IUnitOfWork
{
    private bool _disposed;
    public ApplicationDbContext DbContext { get; }
    public UnitOfWork(ApplicationDbContext dbContext) => DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<Result> SaveChangesAsync()
    {
        try
        {
            await DbContext.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(InfrastructureErrors.SaveChangesError(e));
        }
    }
    public Result SaveChanges()
    {
        try
        {
            DbContext.SaveChanges();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(InfrastructureErrors.SaveChangesError(e));
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
        }

        _disposed = true;
    }
}
