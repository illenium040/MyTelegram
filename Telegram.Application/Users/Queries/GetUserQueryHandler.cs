using Telegram.Application.Abstractions;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Application.Users.Queries
{
    internal class GetUserQueryHandler : QueryHandler<GetUserQuery, User>
    {
        private readonly IUserRepository _userRepository;
        public GetUserQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = userRepository;
        }

        public override async Task<Result<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var res = request switch
            {
                { Id: not null } => await _userRepository.GetByIdAsync(request.Id.Value),
                { UserName: not null } => await _userRepository.GetByNameAsync(request.UserName),
                { DisplayName: not null } => await _userRepository.GetByDisplayNameAsync(request.DisplayName),
                _ => throw new InvalidOperationException()
            };

            if (res is null) return Result.Failure<User>(new Error("GetUser", "User not found"));

            return Result.Success<User>(res);
        }
    }
}
