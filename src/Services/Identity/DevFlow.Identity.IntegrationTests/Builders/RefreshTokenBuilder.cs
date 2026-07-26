namespace DevFlow.Identity.IntegrationTests.Builders;

public static class RefreshTokenBuilder
{
    public static string Token =>
        Guid.NewGuid().ToString("N");
}
