namespace DevFlow.Identity.Infrastructure.Seed;

internal static class DemoUsers
{
    public static IReadOnlyList<DemoUser> All =>
    [
        new(
            Email: "admin@devflow.com",
            Password: "DevFlow@123",
            FirstName: "System",
            LastName: "Administrator"),

        new(
            Email: "pm@devflow.com",
            Password: "DevFlow@123",
            FirstName: "Chintan",
            LastName: "Chhapgar"),

        new(
            Email: "scrum@devflow.com",
            Password: "DevFlow@123",
            FirstName: "John",
            LastName: "Smith"),

        new(
            Email: "sarah@devflow.com",
            Password: "DevFlow@123",
            FirstName: "Sarah",
            LastName: "Johnson"),

        new(
            Email: "michael@devflow.com",
            Password: "DevFlow@123",
            FirstName: "Michael",
            LastName: "Brown"),

        new(
            Email: "emily@devflow.com",
            Password: "DevFlow@123",
            FirstName: "Emily",
            LastName: "Davis"),

        new(
            Email: "david@devflow.com",
            Password: "DevFlow@123",
            FirstName: "David",
            LastName: "Wilson"),

        new(
            Email: "olivia@devflow.com",
            Password: "DevFlow@123",
            FirstName: "Olivia",
            LastName: "Taylor")
    ];
}

internal sealed record DemoUser(
    string Email,
    string Password,
    string FirstName,
    string LastName);
