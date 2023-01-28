using Microsoft.EntityFrameworkCore;

namespace Telegram.Domain.Abstractions;

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    TContext DbContext { get; }
}
