using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Identity.Infrastructure.Persistence.Repositories;

internal sealed class SecurityEventRepository
    : ISecurityEventRepository
{
    private readonly IdentityDbContext _context;

    public SecurityEventRepository(
        IdentityDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        SecurityEvent securityEvent,
        CancellationToken cancellationToken)
    {
        await _context.SecurityEvents.AddAsync(
            securityEvent,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<IReadOnlyList<SecurityEvent>> GetByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken)
    {
        return await _context.SecurityEvents
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.OccurredOnUtc)
            .ToListAsync(cancellationToken);
    }
}
