using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Domain.Primitivies;
using Telegram.Domain.Shared;

namespace Telegram.Domain.ValueObjects
{
    public class Email : ValueObject<string>
    {
        public static readonly int MaxLength = 50;
        private Email(string email) : base(email)  { }

        // This method is used for validation
        public static bool IsValid(string email) => Create(email).IsSuccess;

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<Email>(new Error(
                    "Email.Create",
                    "Email is empty"
                ));
            }

            if (!email.Contains("@"))
            {
                return Result.Failure<Email>(new Error(
                     "Email.Create",
                     "Email must contains @"
                 ));
            }

            return Result.Success<Email>(new(email));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
