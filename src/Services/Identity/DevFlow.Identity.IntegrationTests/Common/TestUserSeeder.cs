using Bogus;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.Identity.Infrastructure.Persistence;

namespace DevFlow.Identity.IntegrationTests.Common;

public sealed class TestUserSeeder
{
    private readonly IdentityDbContext _db;
    private readonly Faker _faker = new();

    public TestUserSeeder(
        IdentityDbContext db)
    {
        _db = db;
    }

    public async Task<TestUser> CreateAsync(
        string? email = null,
        string password = "Password@123")
    {
        email ??= _faker.Internet.Email().ToLowerInvariant();

        var firstName = _faker.Name.FirstName();
        var lastName = _faker.Name.LastName();

        var user = User.Create(
            email,
            TestPasswordHasher.Hash(password),
            firstName,
            lastName);

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        return new TestUser(
            user.Id.Value,
            email,
            password,
            firstName,
            lastName);
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(
            new UserId(id));

        return user
            ?? throw new InvalidOperationException(
                $"User '{id}' was not found.");
    }
}
