using FluentValidation;
using Telegram.Domain.ValueObjects;

namespace Telegram.Application.Users.Commands
{
    internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty().NotNull()
                .Must(Email.IsValid)
                .MaximumLength(Email.MaxLength);
            RuleFor(c => c.Login)
                .NotEmpty().NotNull()
                .MaximumLength(30);
            RuleFor(c => c.DisplayName)
                .NotEmpty().NotNull()
                .Must(DisplayName.IsValid)
                .MinimumLength(DisplayName.MinLength)
                .MaximumLength(DisplayName.MaxLength);
            RuleFor(c => c.Password)
                .NotEmpty().NotNull()
                .Must(Password.IsValid)
                .MinimumLength(Password.MinLength)
                .MaximumLength(Password.MaxLength);
        }
    }
}
