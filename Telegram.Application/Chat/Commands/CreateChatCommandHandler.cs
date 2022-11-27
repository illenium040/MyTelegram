using Telegram.Application.Abstractions;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Abstractions;
using ChatEntity = Telegram.Domain.Entities.Chat;

namespace Telegram.Application.Chat.Commands
{
    internal class CreateChatCommandHandler : CommandHandler<CreateChatCommand, ChatEntity>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        public CreateChatCommandHandler(IUnitOfWork unitOfWork,
            IChatRepository chatRepository,
            IUserRepository userRepository)
            : base(unitOfWork)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        public override async Task<Result<ChatEntity>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var initiator = await _userRepository.GetByIdAsync(request.InitiatorUser);
            if (initiator is null) return Result.Failure<ChatEntity>(new("CreateChatCommandHandler.InitiatorUser", "User not found"));
            var receiver = await _userRepository.GetByIdAsync(request.ReceiverUser);
            if (receiver is null) return Result.Failure<ChatEntity>(new("CreateChatCommandHandler.ReceiverUser", "User not found"));

            if (await _chatRepository.IsExisting(initiator, receiver))
            {
                return Result.Failure<ChatEntity>(new("CreateChatCommandHandler", "Chat is already existing"));
            }


            var chat = ChatEntity.Create(request.ChatType);
            _chatRepository.Add(chat);
            _chatRepository.AppendUser(initiator.AddChat(chat.Id));
            _chatRepository.AppendUser(receiver.AddChat(chat.Id));

            var result = await UnitOfWork.SaveChangesAsync();

            if (result.IsFailure) return Result.Failure<ChatEntity>(result.Error);

            return Result.Success(chat);
        }
    }
}
