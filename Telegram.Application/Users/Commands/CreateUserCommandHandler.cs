using Telegram.Application.Abstractions;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;
using Telegram.Domain.ValueObjects;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Application.Users.Commands;

internal sealed class CreateUserCommandHandler : CommandHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    public CreateUserCommandHandler(IUnitOfWork unitOfWork,
        IUserRepository userRepository) : base(unitOfWork) => _userRepository = userRepository;

    public override async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<User>(emailResult.Error!.ConcatCode(nameof(CreateUserCommandHandler)));
        }

        var passwordResult = Password.Create(request.Password);
        if (passwordResult.IsFailure)
        {
            return Result.Failure<User>(passwordResult.Error!.ConcatCode(nameof(CreateUserCommandHandler)));
        }

        var loginResult = Login.Create(request.Login);
        if (loginResult.IsFailure)
        {
            return Result.Failure<User>(loginResult.Error!.ConcatCode(nameof(CreateUserCommandHandler)));
        }

        var aboutResult = About.Create(request.About);
        if (aboutResult.IsFailure)
        {
            return Result.Failure<User>(aboutResult.Error!.ConcatCode(nameof(CreateUserCommandHandler)));
        }

        var displayNameResult = DisplayName.Create(request.DisplayName);
        if (displayNameResult.IsFailure)
        {
            return Result.Failure<User>(aboutResult.Error!.ConcatCode(nameof(CreateUserCommandHandler)));
        }

        var user = User.Create(
            displayNameResult.Value!,
            loginResult.Value!,
            emailResult.Value!,
            passwordResult.Value!,
            request.AvatarLink,
            aboutResult.Value);

        var result = await _userRepository.CreateAsync(user);

        if (result.IsSuccess)
        {
            var saveResult = await UnitOfWork.SaveChangesAsync();
            return saveResult.IsSuccess ? Result.Success(user) : Result.Failure<User>(saveResult.Error);
        }

        return Result.Failure<User>(result.Error);
    }
}
