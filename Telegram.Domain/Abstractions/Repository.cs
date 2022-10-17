using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Primitivies;

namespace Telegram.Domain.Abstractions
{
    public abstract class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected TContext Context { get; }
        protected Repository(TContext context)
        {
            Context = context;
        }
        public virtual void Add(TEntity entity) => Context.Add(entity);

        public virtual IAsyncEnumerable<TEntity?> GetAllAsync() => Context.Set<TEntity>().AsAsyncEnumerable();

        public virtual Task<TEntity?> GetByIdAsync(Guid id) => Context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);

        public virtual void Remove(TEntity entity) => Context.Remove(entity);

        public virtual void Update(TEntity entity) => Context.Update(entity);
    }
}
