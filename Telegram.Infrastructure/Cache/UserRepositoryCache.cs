using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Domain.Entities;
using Telegram.Domain.Primitivies;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Infrastructure.Cash
{
    public class UserRepositoryCache : Repository<User, ApplicationDbContext>, IUserRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        public UserRepositoryCache(IUserRepository userRepository, 
            IMemoryCache memoryCache, 
            ApplicationDbContext context) : base(context)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public void AppendChat(UserChat userChat) => _userRepository.AppendChat(userChat);

        public Task<Result> CreateAsync(User user) => _userRepository.CreateAsync(user);

        public Task<User?> GetByDisplayNameAsync(string displayName)
        {
            var key = $"{nameof(GetByDisplayNameAsync)}-{displayName}";
            return _memoryCache.GetOrCreateAsync(key, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                return _userRepository.GetByDisplayNameAsync(displayName);
            });
        }
        //can be cached
        public Task<User?> GetByEmailAsync(string email)
        {
            var key = $"{nameof(GetByEmailAsync)}-{email}";
            return _memoryCache.GetOrCreateAsync(key, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                return _userRepository.GetByEmailAsync(email);
            });
        }
        //can be cached
        public Task<User?> GetByNameAsync(string userName)
        {
            var key = $"{nameof(GetByNameAsync)}-{userName}";
            return _memoryCache.GetOrCreateAsync(key, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                return _userRepository.GetByNameAsync(userName);
            });
        }
    }
}
