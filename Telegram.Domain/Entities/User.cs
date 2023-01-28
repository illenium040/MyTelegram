using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Domain.DomainEvents;
using Telegram.Domain.Primitivies;
using Telegram.Domain.ValueObjects;

namespace Telegram.Domain.Entities
{
    [Table("Users")]
    public class User : AggregateRoot
    {
        public string Email { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string DisplayName { get; private set; }
        public string? AvatarLink { get; private set; }
        public string? About { get; private set; }


        private List<UserChat> _chats;
        [BackingField(nameof(_chats))]
        public IReadOnlyList<UserChat> UserChats { get => _chats; }

        private List<BlockedUser> _blocked;
        [BackingField(nameof(_blocked))]
        public IReadOnlyList<BlockedUser> BlockedUsers { get => _blocked; }

        internal User(
            Guid id,
            string email, 
            string login,
            string password, 
            string displayName,
            string? avatarLink, 
            string? about) : base(id)
        {
            Email = email;
            Login = login;
            Password = password;
            DisplayName = displayName;
            AvatarLink = avatarLink;
            About = about;
            _chats = new();
            _blocked = new();
        }

        private User(Guid id,
            DisplayName displayName,
            Login login,
            Email email,
            Password password,
            string? avatarLink = null,
            About? about = null)
            :base(id)
        {
            Email = email.Value;
            Login = login.Value;
            DisplayName = displayName.Value;
            AvatarLink = avatarLink;
            About = about.Value;
            Password = password.Value;
            _blocked = new List<BlockedUser>();
            _chats = new List<UserChat>();
        }

        public static User Create(
            DisplayName displayName,
            Login login,
            Email email,
            Password password,
            string? avatarLink = null,
            About? about = null)
        {
            var user = new User(Guid.NewGuid(), displayName, login, email, password, avatarLink, about);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id, login));
            return user;
        }


        public BlockedUser BlockUser(Guid userId)
        {
            var blocked = new BlockedUser(Guid.NewGuid(), userId, Id);
            _blocked.Add(blocked);
            return blocked;
        }

        public UserChat AddChat(Guid chatId,
            bool isArchived = false,
            bool isPinned = false,
            bool isDeleted = false,
            bool isNotifications = true)
        {
            var chat = new UserChat(Guid.NewGuid(), Id, chatId, isArchived, isPinned, isDeleted, isNotifications);
            _chats.Add(chat);
            return chat;
        }
    }
}
