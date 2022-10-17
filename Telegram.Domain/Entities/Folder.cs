using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Domain.Primitivies;

namespace Telegram.Domain.Entities
{
    [Table("Folders")]
    public class Folder : Entity
    {
        public Guid UserChatId { get; private set; }
        public UserChat? UserChat { get; private set; }
        public string Name { get; private set; }
        internal Folder(Guid id, Guid userChatId, string name) : base(id)
        {
            UserChatId = userChatId;
            Name = name;
        }
    }
}
