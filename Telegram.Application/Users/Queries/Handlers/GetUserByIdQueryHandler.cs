using Telegram.Application.Abstractions;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Application.Users.Queries.Handlers;

internal class GetUserByIdQueryHandler : QueryHandler<GetUserByIdQuery, User>
{
    private readonly IUserRepository _userRepository;
    public GetUserByIdQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : base(unitOfWork) => _userRepository = userRepository;

    public override async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _userRepository.GetByIdAsync(request.Id);

        return res is null ? Result.Failure<User>(new Error("GetUser", "User not found")) : Result.Success(res);
    }
}
