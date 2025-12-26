using System.Text.RegularExpressions;

namespace Shared.Helpers;

public static partial class SlugHelper
{
    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex MyRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex1();

    [GeneratedRegex(@"-+")]
    private static partial Regex MyRegex2();

    public static string GenerateSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // 1️⃣ Convert to lowercase
        input = input.ToLowerInvariant();

        // 2️⃣ Remove special characters
        input = MyRegex().Replace(input, "");

        // 3️⃣ Replace spaces with hyphens
        input = MyRegex1().Replace(input, "-");

        // 4️⃣ Remove multiple hyphens
        input = MyRegex2().Replace(input, "-");

        // 5️⃣ Trim hyphens
        return input.Trim('-');
    }

    public static string GenerateUniqueSlug(string input)
    {
        var baseSlug = GenerateSlug(input);
        var randomSuffix = RandomGenerator.GenerateInteger(1000, 9999);

        return $"{baseSlug}-{randomSuffix}";
    }
}
