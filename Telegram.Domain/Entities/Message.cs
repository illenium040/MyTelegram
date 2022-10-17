using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Domain.Enums;
using Telegram.Domain.Primitivies;

namespace Telegram.Domain.Entities
{
    [Table("Messages")]
    public class Message : Entity
    {
        public string Content { get; private set; }
        public Guid ChatId { get; private set; }
        public Chat? Chat { get; private set; }
        public MessageType Type { get; private set; }
        public DateTime Date { get; private set; }
        public Guid ReceiverId { get; private set; }
        public Guid SenderId { get; private set; }
        public Guid? DeletedForId { get; private set; }
        public bool IsPinned { get; private set; }
        public bool IsChanged { get; private set; }
        public Guid? ReplyMessageId { get; private set; }
        internal Message(Guid id,
            Guid chatId,
            DateTime date,
            Guid receiverId,
            Guid senderId,
            string content,
            MessageType type = MessageType.Text,
            bool isPinned = false,
            bool isChanged = false,
            Guid? replyMessageId = null,
            Guid? deletedForId = null) : base(id)
        {
            Type = type;
            Content = content;
            ChatId = chatId;
            Date = date;
            ReceiverId = receiverId;
            SenderId = senderId;
            DeletedForId = deletedForId;
            IsPinned = isPinned;
            IsChanged = isChanged;
            ReplyMessageId = replyMessageId;
        }
    }
}
