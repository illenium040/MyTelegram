using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;
using Telegram.Domain.ValueObjects;

namespace Telegram.Infrastructure.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByDisplayNameAsync(DisplayName displayName);
        Task<User?> GetByLoginAsync(Login login);
        Task<User?> GetByEmailAsync(Email email);

        Task<Result> CreateAsync(User user);

        void AppendChat(UserChat userChat);
    }
}
