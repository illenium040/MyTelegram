using Telegram.Application.Abstractions;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Application.Users.Commands
{

    internal sealed class CreateUserCommandHandler : CommandHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;
        public CreateUserCommandHandler(IUnitOfWork unitOfWork,
            IUserRepository userRepository) : base(unitOfWork)
        {
            _userRepository = userRepository;
        }

        public override async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(
                request.DisplayName,
                request.UserName,
                request.Email,
                request.AvatarLink,
                request.About);

            var result = await _userRepository.CreateAsync(user, request.Password);

            if (result.IsSuccess)
            {
                var saveResult = await UnitOfWork.SaveChangesAsync();
                if (saveResult.IsSuccess) return Result.Success(user);
                return Result.Failure<User>(saveResult.Error);
            }

            return Result.Failure<User>(result.Error);
        }
    }
}
