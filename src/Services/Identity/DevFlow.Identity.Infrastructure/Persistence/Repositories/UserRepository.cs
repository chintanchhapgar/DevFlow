using DevFlow.Identity.Domain.Authentication.Users;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Identity.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of the user repository.
/// </summary>
internal sealed class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(
        UserId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(
                x => x.Email == email,
                cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(
            user,
            cancellationToken);
    }

    public async Task UpdateAsync(
        User user,
    CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);

        Console.WriteLine(_context.ChangeTracker.DebugView.LongView);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var id = new UserId(userId);

        return await _context.Users.AnyAsync(
            x => x.Id == id,
            cancellationToken);
    }
    public async Task<(IReadOnlyList<User> Users, int TotalCount)> GetPagedAsync(
    int page,
    int pageSize,
    string? search,
    CancellationToken cancellationToken)
    {
        IQueryable<User> query = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                EF.Functions.ILike(x.FirstName, $"%{search}%") ||
                EF.Functions.ILike(x.LastName, $"%{search}%") ||
                EF.Functions.ILike(x.Email, $"%{search}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (users, totalCount);
    }

    public async Task<User?> GetByEmailVerificationTokenAsync(
    Guid token,
    CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                x => x.EmailVerificationToken == token,
                cancellationToken);
    }
    public async Task<IReadOnlyList<User>> GetByIdsAsync(
    IReadOnlyCollection<Guid> userIds,
    CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
        {
            return [];
        }

        var ids = userIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .Select(id => new UserId(id))
            .ToArray();

        if (ids.Length == 0)
        {
            return [];
        }

        return await _context.Users
            .AsNoTracking()
            .Where(user => ids.Contains(user.Id))
            .ToListAsync(cancellationToken);
    }
}
