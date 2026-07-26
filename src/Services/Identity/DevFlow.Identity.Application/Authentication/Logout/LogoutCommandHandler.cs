using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Logout;

internal sealed class LogoutCommandHandler
    : IRequestHandler<LogoutCommand, Result<LogoutResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ISecurityEventLogger _securityEventLogger;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        ISecurityEventLogger securityEventLogger
        )
    {
        _refreshTokenRepository = refreshTokenRepository;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<LogoutResponse>> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        var refreshToken =
            await _refreshTokenRepository.GetByTokenAsync(
                request.RefreshToken,
                cancellationToken);

        // Idempotent logout:
        // If the token doesn't exist or is already inactive,
        // still return success.
        if (refreshToken is null)
        {
            return new LogoutResponse();
        }

        if (refreshToken.IsActive)
        {
            refreshToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(
                refreshToken,
                cancellationToken);
        }

        await _securityEventLogger.LogAsync(
            refreshToken.UserId,
            SecurityEventType.Logout,
            cancellationToken: cancellationToken);

        return new LogoutResponse();
    }
}
