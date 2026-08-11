using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeOtherSessions;

internal sealed class RevokeOtherSessionsCommandHandler
    : IRequestHandler<
        RevokeOtherSessionsCommand,
        Result<RevokeOtherSessionsResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ICurrentUser _currentUser;
    private readonly ISecurityEventLogger _securityEventLogger;

    public RevokeOtherSessionsCommandHandler(
        IRefreshTokenRepository refreshTokens,
        ICurrentUser currentUser,
        ISecurityEventLogger securityEventLogger)
    {
        _refreshTokens = refreshTokens;
        _currentUser = currentUser;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<RevokeOtherSessionsResponse>> Handle(
        RevokeOtherSessionsCommand request,
        CancellationToken cancellationToken)
    {
        var sessions =
    await _refreshTokens.GetActiveOtherSessionsAsync(
        new UserId(_currentUser.UserId),
        _currentUser.SessionId,
        cancellationToken);

        var sessionCount = sessions
            .Select(x => x.SessionId)
            .Distinct()
            .Count();

        foreach (var token in sessions)
        {
            token.Revoke(
                reason: "Other sessions revoked");
        }

        await _refreshTokens.UpdateRangeAsync(
            sessions,
            cancellationToken);

        await _securityEventLogger.LogAsync(
            new UserId(_currentUser.UserId),
            SecurityEventType.OtherSessionsRevoked,
            cancellationToken: cancellationToken);

        return Result.Success(
            new RevokeOtherSessionsResponse(
                sessionCount,
                "Other sessions revoked successfully."));
    }
}
