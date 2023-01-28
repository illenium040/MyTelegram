namespace Telegram.Domain.Shared;

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

    public static bool operator ==(Error? a, Error? b) => (a is not null || b is not null) && a is not null && b is not null && (a.Equals(b) || a.Code == b.Code);

    public static bool operator !=(Error? a, Error? b) => (a is not null || b is not null) && a is not null && b is not null && (!a.Equals(b) || a.Code != b.Code);

    public override bool Equals(object? obj) => obj is not null && obj.GetType() == typeof(Error);
    public bool Equals(Error? other) => other is not null && other.GetType() == typeof(Error) && other is Error error && Code == other.Code;

    public override int GetHashCode() => HashCode.Combine(Code, Message);

    public override string ToString() => this == None || this == Null ? "" : $"{Code}: {Message}";

}
