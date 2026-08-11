using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Common;

namespace DevFlow.Identity.Infrastructure.Authentication;

internal sealed class SessionValidator : ISessionValidator
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public SessionValidator(
        IRefreshTokenRepository refreshTokenRepository)
    {
        ArgumentNullException.ThrowIfNull(refreshTokenRepository);

        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> IsSessionActiveAsync(
        Guid userId,
        Guid sessionId,
        CancellationToken cancellationToken = default)
    {
        var refreshTokens =
            await _refreshTokenRepository.GetBySessionIdAsync(
                sessionId,
                cancellationToken);

        return refreshTokens.Any(
            token =>
                token.UserId.Value == userId &&
                token.IsActive);
    }
}
