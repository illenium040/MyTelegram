using FluentValidation;

namespace Telegram.Application.Users.Commands
{
    internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty().NotNull()
                .Must(x => x.Contains('@'));
            RuleFor(c => c.UserName)
                .NotEmpty().NotNull()
                .MaximumLength(30);
            RuleFor(c => c.DisplayName)
                .NotEmpty().NotNull()
                .MaximumLength(30);
            RuleFor(c => c.Password)
                .NotEmpty().NotNull()
                .MaximumLength(20);
        }
    }
}
