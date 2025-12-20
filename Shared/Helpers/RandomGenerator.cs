namespace Shared.Helpers;

public static class RandomGenerator
{
    private static readonly Random random = new Random();

    public static int GenerateInteger(int min, int max)
    {
        return random.Next(min, max);
    }

    public static string RandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        return new string(
            Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()
        );
    }
}
