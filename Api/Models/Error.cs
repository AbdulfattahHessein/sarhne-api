namespace Api.Models;

public sealed record Error(string Code)
{
    public static implicit operator string(Error error) => error.Code;

    public static implicit operator Error(string code) => new(code);
}

public static class Errors
{
    public static readonly Error None = string.Empty;
    public static readonly Error NotFound = nameof(NotFound);
    public static readonly Error Validation = nameof(Validation);
    public static readonly Error Unauthorized = nameof(Unauthorized);
    public static readonly Error Internal = nameof(Internal);
}
