using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Domain.Primitivies;

namespace Telegram.Domain.Entities
{
    [Table("UserChats")]
    //Use this aggregate root instaed of user
    public class UserChat : Entity
    {
        public Guid UserId { get; private set; }
        public User? User { get; private set; }
        public Guid ChatId { get; private set; }
        public Chat? Chat { get; private set; }
        public bool IsArchived { get; private set; }
        public bool IsPinned { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool IsNotifications { get; private set; }

        private List<Folder> _folders;
        [BackingField(nameof(_folders))]
        public IReadOnlyList<Folder> Folders { get => _folders; }

      

        internal UserChat(Guid id, Guid userId, Guid chatId, bool isArchived, bool isPinned, bool isDeleted, bool isNotifications)
            :base(id)
        {
            UserId = userId;
            ChatId = chatId;
            IsArchived = isArchived;
            IsPinned = isPinned;
            IsDeleted = isDeleted;
            IsNotifications = isNotifications;
            _folders = new List<Folder>();
        }

        public Folder CreateFolder(string name)
        {
            var folder = new Folder(Guid.NewGuid(), Id, name);
            _folders.Add(folder);
            return folder;
        }
    }
}
