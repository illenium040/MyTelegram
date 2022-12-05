using Newtonsoft.Json.Linq;
using Telegram.Application.Abstractions;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;
using Telegram.Domain.ValueObjects;
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
            var loginResult = Login.Create(request.Login);
            var displayNameResult = DisplayName.Create(request.DisplayName);
            var res = request switch
            {
                { Id: not null } => await _userRepository.GetByIdAsync(request.Id.Value),
                { Login: not null } => await _userRepository.GetByLoginAsync(loginResult.Value!),
                { DisplayName: not null } => await _userRepository.GetByDisplayNameAsync(displayNameResult.Value!),
                _ => throw new InvalidOperationException()
            };

            if (res is null) return Result.Failure<User>(new Error("GetUser", "User not found"));

            return Result.Success<User>(res);
        }
    }
}
