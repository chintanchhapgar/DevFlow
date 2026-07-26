using DevFlow.SharedKernel.Common;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Results;
using MediatR;
using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeOtherSessions;

internal sealed class RevokeOtherSessionsCommandHandler
    : IRequestHandler<
        RevokeOtherSessionsCommand,
        Result<RevokeOtherSessionsResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ICurrentUser _currentUser;

    public RevokeOtherSessionsCommandHandler(
        IRefreshTokenRepository refreshTokens,
        ICurrentUser currentUser)
    {
        _refreshTokens = refreshTokens;
        _currentUser = currentUser;
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

        foreach (var token in sessions)
        {
            token.Revoke(
                reason: "Other sessions revoked");
        }

        await _refreshTokens.UpdateRangeAsync(
            sessions,
            cancellationToken);

        return Result.Success(
            new RevokeOtherSessionsResponse(
                sessions.Count,
                "Other sessions revoked successfully."));
    }
}
