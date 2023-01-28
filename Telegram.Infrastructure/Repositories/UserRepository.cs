using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Entities;
using Telegram.Domain.Primitivies;
using Telegram.Domain.Shared;
using Telegram.Domain.ValueObjects;
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

        public Task<User?> GetByDisplayNameAsync(DisplayName displayName)
            => _users.SingleOrDefaultAsync(x => x.DisplayName == displayName.Value);

        public Task<User?> GetByLoginAsync(Login login)
            => _users.SingleOrDefaultAsync(x => x.Login == login.Value);

        public Task<User?> GetByEmailAsync(Email email)
            => _users.SingleOrDefaultAsync(x => x.Email == email.Value);

        public void AppendChat(UserChat userChat) => _usersChats.Add(userChat);
    }
}
