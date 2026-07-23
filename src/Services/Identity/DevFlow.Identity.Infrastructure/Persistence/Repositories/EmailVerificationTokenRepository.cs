using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.EmailVerificationTokens;
using DevFlow.Identity.Domain.Authentication.Users;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Identity.Infrastructure.Persistence.Repositories;

internal sealed class EmailVerificationTokenRepository
    : IEmailVerificationTokenRepository
{
    private readonly IdentityDbContext _context;

    public EmailVerificationTokenRepository(
        IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<EmailVerificationToken?> GetByTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        return await _context.EmailVerificationTokens
            .FirstOrDefaultAsync(
                x => x.Token == token,
                cancellationToken);
    }

    public async Task<List<EmailVerificationToken>> GetActiveByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.EmailVerificationTokens
            .Where(x =>
                x.UserId == userId &&
                x.Status == EmailVerificationStatus.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        EmailVerificationToken token,
        CancellationToken cancellationToken = default)
    {
        await _context.EmailVerificationTokens.AddAsync(
            token,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        EmailVerificationToken token,
        CancellationToken cancellationToken = default)
    {
        _context.EmailVerificationTokens.Update(token);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
