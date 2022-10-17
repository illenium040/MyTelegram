using Telegram.Domain.Shared;

namespace Telegram.Domain.Abstractions
{
    public interface IValidationResult
    {
        public static readonly Error ValidationError = new(
            "ValidationError",
            "Validation error occured");

        Error[] Errors { get; }
    }
}
