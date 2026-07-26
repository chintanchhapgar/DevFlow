using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Sessions.GetSessions;

internal sealed class GetSessionsQueryHandler
    : IRequestHandler<
        GetSessionsQuery,
        Result<IReadOnlyList<SessionResponse>>>
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ICurrentUser _currentUser;

    public GetSessionsQueryHandler(
        IRefreshTokenRepository refreshTokens,
        ICurrentUser currentUser)
    {
        _refreshTokens = refreshTokens;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyList<SessionResponse>>> Handle(
        GetSessionsQuery request,
        CancellationToken cancellationToken)
    {
        var sessions = await _refreshTokens.GetByUserIdAsync(
            new UserId(_currentUser.UserId),
            cancellationToken);

        var response = sessions
            .Select(x => new SessionResponse(
                x.Id.Value,
                x.DeviceName,
                x.Browser,
                x.OperatingSystem,
                x.IpAddress,
                x.CreatedOnUtc,
                x.LastUsedOnUtc,
                x.ExpiresOnUtc,
                x.Id.Value == _currentUser.SessionId))
            .ToList();

        return Result.Success<IReadOnlyList<SessionResponse>>(response);
    }
}
