namespace Telegram.Domain.Shared;

public static class ErrorExtensions
{
    private static string ConcatWithNewLine(string s1, string s2)
        => string.Concat(s1, "\n", s2);

    public static Error ConcatCode(this Error error, string code)
        => new(ConcatWithNewLine(code, error.Code), error.Message);

}
