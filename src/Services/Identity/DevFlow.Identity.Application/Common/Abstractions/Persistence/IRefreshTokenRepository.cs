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

    Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(
        IEnumerable<RefreshToken> refreshTokens,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RefreshToken>> GetByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default);

    Task<List<RefreshToken>> GetBySessionIdAsync(
        Guid sessionId,
        CancellationToken cancellationToken);

    Task<List<RefreshToken>> GetActiveOtherSessionsAsync(
        UserId userId,
        Guid currentSessionId,
        CancellationToken cancellationToken);
    Task<List<RefreshToken>> GetByUserIdAndSessionIdAsync(
        UserId userId,
        Guid sessionId,
        CancellationToken cancellationToken = default);

}
