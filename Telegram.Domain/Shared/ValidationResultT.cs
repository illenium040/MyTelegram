using Telegram.Domain.Abstractions;

namespace Telegram.Domain.Shared
{
    public sealed class ValidationResult<T> : Result<T>, IValidationResult
    {
        internal ValidationResult(Error[] errors)
            : base(default, false, IValidationResult.ValidationError)
            => Errors = errors;

        public Error[] Errors { get; }

        public static ValidationResult<T> WithErrors(Error[] errors) => new(errors);
    }
}
