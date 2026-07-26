using Bogus;

namespace DevFlow.Identity.IntegrationTests.Builders;

public static class UserBuilder
{
    private static readonly Faker Faker = new();

    public static string Email =>
        Faker.Internet.Email();

    public static string Password =>
        "Password@123";

    public static string FirstName =>
        Faker.Name.FirstName();

    public static string LastName =>
        Faker.Name.LastName();
}
