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
    public class DisplayName : ValueObject<string>
    {
        public static readonly int MaxLength = 20;
        public static readonly int MinLength = 2;

        private static readonly string _regexPattern = $"^[a-zA-Z0-9_.-]{{{MinLength},{MaxLength}}}$";

        private DisplayName(string value) : base(value) { }

        public static bool IsValid(string? displayName) => Create(displayName).IsSuccess;

        public static Result<DisplayName> Create(string? displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                return Result.Failure<DisplayName>(new Error(
                    "DisplayName.Create",
                    "DisplayName is empty"
                    ));
            }

            if (!Regex.Match(displayName, _regexPattern).Success) 
            {
                return Result.Failure<DisplayName>(new Error(
                    "DisplayName.Create",
                    $"DisplayName allowed to have only letters and digits in display name with min length of {MinLength} and max length of {MaxLength}"
                    ));
            }

            return Result.Success<DisplayName>(new(displayName));
        }


        public override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
