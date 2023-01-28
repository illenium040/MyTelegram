using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Domain.Primitivies;
using Telegram.Domain.Shared;

namespace Telegram.Domain.ValueObjects
{
    public class Password : ValueObject<string>
    {
        public static readonly int MaxLength = 20;
        public static readonly int MinLength = 8;

        // Minimum MinLength and maximum MaxLength characters, at least one uppercase letter, one lowercase letter, one number and one special character
        private static readonly string _regexPattern = $"^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{{{MinLength},{MaxLength}}}$";

        [JsonConstructor]
        private Password(string value) : base(value) { }

        public static bool IsValid(string? password) => Create(password).IsSuccess;

        public static Result<Password> Create(string? password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Result.Failure<Password>(new Error(
                    "Password.Create",
                    "Password is empty"
                    ));
            }

            if (!Regex.Match(password, _regexPattern).Success)
            {
                return Result.Failure<Password>(new Error(
                    "Password.Create",
                    $"Password allowed to have minimum {MinLength} and maximum {MaxLength} characters, at least one uppercase letter, " +
                    $"one lowercase letter, one number and one special character"
                    ));
            }

            return Result.Success<Password>(new(password));
        }


        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
