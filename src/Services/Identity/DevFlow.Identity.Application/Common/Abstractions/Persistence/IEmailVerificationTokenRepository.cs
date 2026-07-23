using DevFlow.Identity.Domain.Authentication.EmailVerificationTokens;
using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Common.Abstractions.Persistence;

public interface IEmailVerificationTokenRepository
{
    Task<EmailVerificationToken?> GetByTokenAsync(
        string token,
        CancellationToken cancellationToken = default);

    Task<List<EmailVerificationToken>> GetActiveByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        EmailVerificationToken token,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        EmailVerificationToken token,
        CancellationToken cancellationToken = default);
}
