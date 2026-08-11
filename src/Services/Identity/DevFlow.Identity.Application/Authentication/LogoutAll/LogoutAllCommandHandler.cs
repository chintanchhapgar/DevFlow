using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.LogoutAll;

internal sealed class LogoutAllCommandHandler
    : IRequestHandler<LogoutAllCommand, Result<LogoutAllResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ICurrentUser _currentUser;
    private readonly ISecurityEventLogger _securityEventLogger;

    public LogoutAllCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        ICurrentUser currentUser,
        ISecurityEventLogger securityEventLogger)
    {
        ArgumentNullException.ThrowIfNull(refreshTokenRepository);
        ArgumentNullException.ThrowIfNull(currentUser);
        ArgumentNullException.ThrowIfNull(securityEventLogger);

        _refreshTokenRepository = refreshTokenRepository;
        _currentUser = currentUser;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<LogoutAllResponse>> Handle(
        LogoutAllCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var refreshTokens =
            await _refreshTokenRepository.GetActiveByUserIdAsync(
                new UserId(userId),
                cancellationToken);

        var revokedSessions = 0;

        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(
                refreshToken,
                cancellationToken);

            revokedSessions++;
        }

        await _securityEventLogger.LogAsync(
            new UserId(userId),
            SecurityEventType.AllSessionsRevoked,
            cancellationToken: cancellationToken);

        return new LogoutAllResponse(
            revokedSessions);
    }
}
