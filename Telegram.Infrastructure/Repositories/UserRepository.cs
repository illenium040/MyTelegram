using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Entities;
using Telegram.Domain.Primitivies;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Infrastructure.Repositories
{
    internal class UserRepository : Repository<User, ApplicationDbContext>, IUserRepository
    {
        private readonly DbSet<User> _users;
        private readonly DbSet<UserChat> _usersChats;
        public UserRepository(ApplicationDbContext ctx) : base(ctx)
        {
            _users = Context.Set<User>();
            _usersChats = Context.Set<UserChat>();
        }
        public override void Add(User entity) =>
            throw new NotImplementedException("This method is not allowed. Use CreateAsync instead.");

        // TODO: Create full user creation with validation and password hashing
        public async Task<Result> CreateAsync(User user)
        {
            var userEmailExist = await _users.SingleOrDefaultAsync(x => x.Email == user.Email) ?? null;
            if (userEmailExist is not null)
            {
                return Result.Failure(new Error("UserRepository.CreateAsync", "User with this email is already exist"));
            }
            var userLoginExist = await _users.SingleOrDefaultAsync(x => x.Login == user.Login);
            if (userLoginExist is not null)
            {
                return Result.Failure(new Error("UserRepository.CreateAsync", "User with this login is already exist"));
            }
            var userEntry = await _users.AddAsync(user);
            return Result.Success(userEntry.Entity);
        }

        public Task<User?> GetByDisplayNameAsync(string displayName)
            => _users.SingleOrDefaultAsync(x => x.DisplayName == displayName);

        public Task<User?> GetByNameAsync(string userName)
            => _users.SingleOrDefaultAsync(x => x.Login == userName);

        public Task<User?> GetByEmailAsync(string email)
            => _users.SingleOrDefaultAsync(x => x.Email == email);

        public void AppendChat(UserChat userChat) => _usersChats.Add(userChat);
    }
}
