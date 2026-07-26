using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeAllSessions;

internal sealed class RevokeAllSessionsCommandHandler
    : IRequestHandler<
        RevokeAllSessionsCommand,
        Result<RevokeAllSessionsResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ICurrentUser _currentUser;
    private readonly ISecurityEventLogger _securityEventLogger;
    public RevokeAllSessionsCommandHandler(
        IRefreshTokenRepository refreshTokens,
        ICurrentUser currentUser,
        ISecurityEventLogger securityEventLogger)
    {
        _refreshTokens = refreshTokens;
        _currentUser = currentUser;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<RevokeAllSessionsResponse>> Handle(
        RevokeAllSessionsCommand request,
        CancellationToken cancellationToken)
    {
        var tokens = await _refreshTokens.GetActiveByUserIdAsync(
            new UserId(_currentUser.UserId),
            cancellationToken);

        foreach (var token in tokens)
        {
            token.Revoke(
                reason: "All sessions revoked");
        }

        await _refreshTokens.UpdateRangeAsync(
            tokens,
            cancellationToken);

        await _securityEventLogger.LogAsync(
             new UserId(_currentUser.UserId),
            SecurityEventType.AllSessionsRevoked,
            cancellationToken: cancellationToken);

        return Result.Success(
            new RevokeAllSessionsResponse(
                tokens.Count,
                "All sessions revoked successfully."));
    }
}
