namespace Telegram.Domain.Shared
{
    public class Result
    {
        protected internal Result(bool isSuccess, Error? error)
        {
            if (isSuccess && error != Error.None) throw new InvalidOperationException();
            if (!isSuccess && error == Error.None) throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error ?? Error.None;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; }

        public static Result Success() => new(true, Error.None);
        public static Result<TValue> Success<TValue>(TValue? value) => new(value, true, Error.None);
        public static Result Failure(Error? error) => new(false, error);
        public static Result Failure(string code, IEnumerable<string> error)
            => new(false, new Error(code, string.Join("; ", error.ToArray())));
        public static Result<TValue> Failure<TValue>(Error? error) => new(default(TValue), false, error);
        public static Result<TValue> Failure<TValue>(string code, IEnumerable<string> error)
            => new(default(TValue), false, new Error(code, string.Join("; ", error.ToArray())));
        public static Result<TValue> Create<TValue>(TValue? value) => new(value, true, Error.None);

        public override string ToString()
        {
            if (Error == Error.Null || Error == Error.None) return "Success";
            return Error!.ToString();
        }

    }
}
