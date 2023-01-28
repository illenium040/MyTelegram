using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Domain.Primitivies;
using Telegram.Domain.Shared;

namespace Telegram.Domain.ValueObjects
{
    public class Login : ValueObject<string>
    {
        public static readonly int MaxLength = 20;
        public static readonly int MinLength = 4;

        private static readonly string _regexPattern = $"^[a-zA-Z0-9_.-]{{{MinLength},{MaxLength}}}$";
        private Login(string value) : base(value) { }

        public static bool IsValid(string? login) => Create(login).IsSuccess;

        public static Result<Login> Create(string? login)
        {
            if (string.IsNullOrEmpty(login))
            {
                return Result.Failure<Login>(new Error(
                    "DisplayName.Create",
                    "DisplayName is empty"
                    ));
            }

            if (!Regex.Match(login, _regexPattern).Success)
            {
                return Result.Failure<Login>(new Error(
                    "DisplayName.Create",
                    $"Login allowed to have only letters and digits in display name with min length of {MinLength} and max length of {MaxLength}"
                    ));
            }

            return Result.Success<Login>(new(login));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
