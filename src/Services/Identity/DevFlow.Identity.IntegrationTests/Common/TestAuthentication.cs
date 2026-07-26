namespace DevFlow.Identity.IntegrationTests.Common;

public sealed record TestAuthentication(
    string AccessToken,
    string RefreshToken);
