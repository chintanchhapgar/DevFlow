namespace DevFlow.Identity.IntegrationTests.Common;

public sealed record TestUser(
    Guid Id,
    string Email,
    string Password,
    string FirstName,
    string LastName);
