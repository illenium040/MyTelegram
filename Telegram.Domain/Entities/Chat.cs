using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Domain.Enums;
using Telegram.Domain.Primitivies;

namespace Telegram.Domain.Entities
{
    [Table("Chats")]
    public class Chat : AggregateRoot
    {
        public ChatType ChatType { get; private set; }

        private List<Message> _messages;
        [BackingField(nameof(_messages))]
        public IReadOnlyList<Message> Messages { get => _messages; }

        private List<UserChat> _userChats;
        [BackingField(nameof(_userChats))]
        public IReadOnlyList<UserChat> UserChats { get => _userChats; }

        private Chat(Guid id, ChatType chatType) : base(id)
        {
            ChatType = chatType;
            _messages = new List<Message>();
            _userChats = new List<UserChat>();
        }

        public static Chat Create(ChatType type = ChatType.Private)
            => new Chat(Guid.NewGuid(), type);

        public Message SendMessage(
            Guid receiverId,
            Guid senderId,
            string content,
            MessageType type = MessageType.Text,
            DateTime? date = null,
            bool isPinned = false,
            bool isChanged = false,
            Guid? replyMessageId = null,
            Guid? deletedForId = null
            )
        {
            var message = new Message(Guid.NewGuid(), Id, date ?? DateTime.UtcNow, receiverId, senderId, content, type,
                isPinned: isPinned,
                isChanged: isChanged,
                replyMessageId: replyMessageId,
                deletedForId: deletedForId);
            _messages.Add(message);
            return message;
        }
    }
}
