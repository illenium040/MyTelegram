using Newtonsoft.Json;
using Telegram.Domain.Primitivies;
using Telegram.Domain.Shared;

namespace Telegram.Domain.ValueObjects;

public class Email : ValueObject<string>
{
    public static readonly int MaxLength = 50;

    [JsonConstructor]
    private Email(string email) : base(email) { }

    // This method is used for validation
    public static bool IsValid(string? email) => Create(email).IsSuccess;

    public static Result<Email> Create(string? email) => string.IsNullOrWhiteSpace(email)
            ? Result.Failure<Email>(new Error(
                "Email.Create",
                "Email is empty"
            ))
            : !email.Contains("@")
            ? Result.Failure<Email>(new Error(
                 "Email.Create",
                 "Email must contains @"
             ))
            : Result.Success<Email>(new(email));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
