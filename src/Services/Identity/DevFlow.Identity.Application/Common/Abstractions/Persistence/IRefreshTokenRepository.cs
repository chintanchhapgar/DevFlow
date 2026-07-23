using DevFlow.Identity.Domain.Authentication.RefreshTokens;
using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Common.Abstractions.Persistence;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(
        string token,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default);

    Task<List<RefreshToken>> GetActiveByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default);
}
