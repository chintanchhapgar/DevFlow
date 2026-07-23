using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.RefreshTokens;
using DevFlow.Identity.Domain.Authentication.Users;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Identity.Infrastructure.Persistence.Repositories;

internal sealed class RefreshTokenRepository
    : IRefreshTokenRepository
{
    private readonly IdentityDbContext _context;

    public RefreshTokenRepository(
        IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(
                x => x.Token == token,
                cancellationToken);
    }

    public async Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        _context.RefreshTokens.Update(refreshToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<RefreshToken>> GetActiveByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Where(x =>
                x.UserId == userId &&
                x.Status == RefreshTokenStatus.Active)
            .ToListAsync(cancellationToken);
    }
}
