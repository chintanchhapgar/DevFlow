using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.PasswordResetTokens;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Identity.Infrastructure.Persistence.Repositories;

internal sealed class PasswordResetTokenRepository
    : IPasswordResetTokenRepository
{
    private readonly IdentityDbContext _context;

    public PasswordResetTokenRepository(
        IdentityDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        PasswordResetToken token,
        CancellationToken cancellationToken = default)
    {
        await _context.PasswordResetTokens.AddAsync(
            token,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PasswordResetToken?> GetByTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        return await _context.PasswordResetTokens
            .FirstOrDefaultAsync(
                x => x.Token == token,
                cancellationToken);
    }

    public async Task UpdateAsync(
        PasswordResetToken token,
        CancellationToken cancellationToken = default)
    {
        _context.PasswordResetTokens.Update(token);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
