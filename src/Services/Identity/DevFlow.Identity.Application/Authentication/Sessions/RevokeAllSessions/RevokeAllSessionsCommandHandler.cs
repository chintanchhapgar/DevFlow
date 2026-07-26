using DevFlow.SharedKernel.Common;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Results;
using MediatR;
using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeAllSessions;

internal sealed class RevokeAllSessionsCommandHandler
    : IRequestHandler<
        RevokeAllSessionsCommand,
        Result<RevokeAllSessionsResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ICurrentUser _currentUser;

    public RevokeAllSessionsCommandHandler(
        IRefreshTokenRepository refreshTokens,
        ICurrentUser currentUser)
    {
        _refreshTokens = refreshTokens;
        _currentUser = currentUser;
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

        return Result.Success(
            new RevokeAllSessionsResponse(
                tokens.Count,
                "All sessions revoked successfully."));
    }
}
