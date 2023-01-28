using Dasync.Collections;
using Telegram.Application.Abstractions;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Application.Users.Queries.Handlers;

internal class GetAllUsersQueryHandler : QueryHandler<GetAllUsersQuery, IEnumerable<User?>>
{
    private readonly IUserRepository _userRepository;
    public GetAllUsersQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : base(unitOfWork) => _userRepository = userRepository;

    public override async Task<Result<IEnumerable<User?>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var res = await _userRepository.GetAllAsync().ToListAsync(cancellationToken);

        return res is null ? Result.Failure<IEnumerable<User?>>(new Error("GetAllUsers", "No users")) : Result.Success(res.AsEnumerable());
    }
}
