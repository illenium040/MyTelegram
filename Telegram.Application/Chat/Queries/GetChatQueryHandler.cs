using Telegram.Application.Abstractions;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Abstractions;
using ChatEntity = Telegram.Domain.Entities.Chat;

namespace Telegram.Application.Chat.Queries;

internal class GetChatQueryHandler : QueryHandler<GetChatQuery, ChatEntity>
{
    private readonly IChatRepository _chatRepository;
    public GetChatQueryHandler(IUnitOfWork unitOfWork, IChatRepository chatRepository) : base(unitOfWork) => _chatRepository = chatRepository;

    public override async Task<Result<ChatEntity>> Handle(GetChatQuery request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.GetByIdAsync(request.Id);

        return chat is null ? Result.Failure<ChatEntity>(new("GetChatQueryHandler", "Chat not found!")) : Result.Success(chat);
    }
}
