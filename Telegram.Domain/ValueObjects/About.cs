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
    public class About : ValueObject<string>
    {
        public static readonly int MaxLength = 150;

        private About(string value) : base(value) { }

        public static Result<About> Create(string? about)
        {
            if (about?.Length > MaxLength)
            {
                return Result.Failure<About>(new Error(
                    "About.Create",
                    $"Length of about is more than {MaxLength}"
                    ));
            }

            return Result.Success<About>(new(about ?? ""));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
