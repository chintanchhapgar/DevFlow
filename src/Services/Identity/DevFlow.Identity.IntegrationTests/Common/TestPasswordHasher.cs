using BCrypt.Net;

namespace DevFlow.Identity.IntegrationTests.Common;

public static class TestPasswordHasher
{
    public static string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
