using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;

namespace Telegram.Infrastructure.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByDisplayNameAsync(string displayName);
        Task<User?> GetByNameAsync(string userName);
        Task<User?> GetByEmailAsync(string email);

        Task<Result> CreateAsync(User user, string password);

        void AppendChat(UserChat userChat);
    }
}
