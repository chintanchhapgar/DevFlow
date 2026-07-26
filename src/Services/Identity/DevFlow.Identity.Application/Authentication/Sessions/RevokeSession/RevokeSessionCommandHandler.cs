using DevFlow.SharedKernel.Common;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Results;
using MediatR;
using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeSession;

internal sealed class RevokeSessionCommandHandler
    : IRequestHandler<
        RevokeSessionCommand,
        Result<RevokeSessionResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ICurrentUser _currentUser;

    public RevokeSessionCommandHandler(
        IRefreshTokenRepository refreshTokens,
        ICurrentUser currentUser)
    {
        _refreshTokens = refreshTokens;
        _currentUser = currentUser;
    }

    public async Task<Result<RevokeSessionResponse>> Handle(
        RevokeSessionCommand request,
        CancellationToken cancellationToken)
    {
        var tokens = await _refreshTokens.GetBySessionIdAsync(
            request.SessionId,
            cancellationToken);

        if (tokens.Count == 0)
        {
            return Result.Failure<RevokeSessionResponse>(
                UserErrors.InvalidRefreshToken);
        }

        if (tokens[0].UserId != new UserId(_currentUser.UserId))
        {
            return Result.Failure<RevokeSessionResponse>(
                UserErrors.Unauthorized);
        }


        foreach (var token in tokens)
        {
            token.Revoke(
                reason: "Session revoked");
        }

        await _refreshTokens.UpdateRangeAsync(
            tokens,
            cancellationToken);

        return Result.Success(
            new RevokeSessionResponse(
                request.SessionId,
                "Session revoked successfully."));
    }
}
