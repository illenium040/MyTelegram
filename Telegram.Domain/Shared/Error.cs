namespace Telegram.Domain.Shared
{
    public class Error : IEquatable<Error>
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error Null = new("Error.Null", "The result value is null");

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; }
        public string Message { get; }

        public static bool operator ==(Error? a, Error? b)
        {
            if (a is null && b is null) return false;
            if (a is null || b is null) return false;
            if (a.Equals(b)) return true;
            return a.Code == b.Code;
        }

        public static bool operator !=(Error? a, Error? b)
        {
            if (a is null && b is null) return false;
            if (a is null || b is null) return false;
            if (!a.Equals(b)) return true;
            return a.Code != b.Code;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != typeof(Error)) return false;
            return true;
        }
        public bool Equals(Error? other)
        {
            if (other is null) return false;
            if (other.GetType() != typeof(Error)) return false;
            if (other is not Error error) return false;
            return Code == other.Code;
        }

        public override int GetHashCode() => HashCode.Combine(Code, Message);

        public override string ToString()
        {
            if (this == None || this == Null) return "";
            return $"{Code}: {Message}";
        }

    }
}
