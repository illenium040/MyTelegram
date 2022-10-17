using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Domain.Primitivies;

namespace Telegram.Domain.Entities
{
    [Table("BlockedUsers")]
    public class BlockedUser : Entity
    {
        public Guid BlockedUserId { get; private set; }
        public Guid BlockedByUserId { get; private set; }
        public User? User { get; private set; }
        internal BlockedUser(Guid id, Guid blockedUserId, Guid blockedByUserId) : base(id)
        {
            BlockedUserId = blockedUserId;
            BlockedByUserId = blockedByUserId;
        }
    }
}
