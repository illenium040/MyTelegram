using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Domain.Primitivies;

namespace Telegram.Domain.Entities
{
    [Table("AspNetUsers")]
    public class User : IdentityUser<Guid>, IEntity
    {
        public string DisplayName { get; private set; }
        public string? AvatarLink { get; private set; }
        public string? About { get; private set; }

        private List<UserChat> _chats;
        [BackingField(nameof(_chats))]
        public IReadOnlyList<UserChat> UserChats { get => _chats; }

        private List<BlockedUser> _blocked;
        [BackingField(nameof(_blocked))]
        public IReadOnlyList<BlockedUser> BlockedUsers { get => _blocked; }

        private User(Guid id,
            string displayName,
            string userName,
            string email,
            string? avatarLink = null,
            string? about = null)
        {
            Id = id;
            Email = email;
            UserName = userName;
            DisplayName = displayName;
            AvatarLink = avatarLink;
            About = about;
            _blocked = new List<BlockedUser>();
            _chats = new List<UserChat>();
        }

        public static User Create(
            string displayName,
            string userName,
            string email,
            string? avatarLink = null,
            string? about = null)
           => new User(Guid.NewGuid(), displayName, userName, email, avatarLink, about);


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

        public bool Equals(IEntity? other)
        {
            if (other is null) return false;
            if (other.GetType() != GetType()) return false;
            if (other is not Entity entity) return false;
            return other.Id == entity.Id;
        }
    }
}
