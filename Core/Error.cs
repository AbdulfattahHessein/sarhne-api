namespace Core;

public sealed record Error(string Code)
{
    public static readonly Error None = new(string.Empty);

    public static readonly Error NotFound = new(nameof(NotFound));
    public static readonly Error Validation = new(nameof(Validation));
    public static readonly Error Unauthorized = new(nameof(Unauthorized));

    public static readonly Error Internal = new(nameof(Internal));
}
