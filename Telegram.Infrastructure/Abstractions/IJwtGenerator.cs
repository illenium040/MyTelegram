using Telegram.Domain.Entities;

namespace Telegram.Infrastructure.Abstractions
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}
