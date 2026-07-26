using DevFlow.Identity.Infrastructure.Persistence;

namespace DevFlow.Identity.IntegrationTests.Common;

public static class TestDataSeeder
{
    public static void Seed(
        IdentityDbContext db)
    {
        if (db.Users.Any())
            return;

        db.SaveChanges();
    }
}
