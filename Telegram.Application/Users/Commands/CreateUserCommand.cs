using Telegram.Application.Abstractions;
using Telegram.Domain.Entities;

namespace Telegram.Application.Users.Commands
{
    public sealed record CreateUserCommand(
        string Email,
        string UserName,
        string DisplayName,
        string Password,
        string? AvatarLink = null,
        string? About = null) : ICommand<User>;
}
