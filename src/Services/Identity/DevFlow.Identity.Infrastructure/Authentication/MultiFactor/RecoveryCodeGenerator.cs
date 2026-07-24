using System.Security.Cryptography;

namespace DevFlow.Identity.Infrastructure.Authentication.MultiFactor;

/// <summary>
/// Generates one-time recovery codes.
/// </summary>
internal static class RecoveryCodeGenerator
{
    private const string Alphabet =
        "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public static IReadOnlyList<string> Generate(
        int count = 10)
    {
        var codes = new List<string>(count);

        for (int i = 0; i < count; i++)
        {
            codes.Add(CreateCode());
        }

        return codes;
    }

    private static string CreateCode()
    {
        Span<char> chars = stackalloc char[10];

        for (int i = 0; i < chars.Length; i++)
        {
            chars[i] = Alphabet[
                RandomNumberGenerator.GetInt32(
                    Alphabet.Length)];
        }

        return $"{new string(chars[..5])}-{new string(chars[5..])}";
    }
}
