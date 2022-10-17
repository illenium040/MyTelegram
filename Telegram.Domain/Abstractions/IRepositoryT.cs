using Telegram.Domain.Primitivies;

namespace Telegram.Domain.Abstractions
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        IAsyncEnumerable<TEntity?> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Guid id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
