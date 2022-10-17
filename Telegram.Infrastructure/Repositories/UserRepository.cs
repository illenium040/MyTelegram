using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Infrastructure.Repositories
{
    internal class UserRepository : Repository<User, ApplicationDbContext>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly DbSet<User> _users;
        private readonly DbSet<UserChat> _usersChats;
        public UserRepository(ApplicationDbContext ctx, UserManager<User> manager) : base(ctx)
        {
            _userManager = manager;
            _users = Context.Set<User>();
            _usersChats = Context.Set<UserChat>();
        }
        public override void Add(User entity) =>
            throw new NotImplementedException("This method is not allowed. Use CreateAsync instead.");

        public async Task<Result> CreateAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded) return Result.Success();
            return Result.Failure("UserRepository.CreateAsync", result.Errors.Select(x => x.Description));
        }

        public Task<User?> GetByDisplayNameAsync(string displayName)
            => _users.SingleOrDefaultAsync(x => x.DisplayName == displayName);

        public Task<User?> GetByNameAsync(string userName)
            => _users.SingleOrDefaultAsync(x => x.UserName == userName);

        public Task<User?> GetByEmailAsync(string email)
            => _users.SingleOrDefaultAsync(x => x.Email == email);

        public void AppendChat(UserChat userChat) => _usersChats.Add(userChat);
    }
}
