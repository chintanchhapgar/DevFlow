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
        var refreshTokens =
            await _refreshTokens.GetByUserIdAsync(
                new UserId(_currentUser.UserId),
                cancellationToken);

        var response = refreshTokens
            // Only sessions which still have an active refresh token
            .Where(x => x.IsActive)

            // Multiple refresh tokens can belong to one session
            .GroupBy(x => x.SessionId)

            .Select(group =>
            {
                // Use the most recently used token as the
                // representative record for this session.
                var token = group
                    .OrderByDescending(
                        x => x.LastUsedOnUtc ?? x.CreatedOnUtc)
                    .First();

                return new SessionResponse(
                    SessionId: group.Key,
                    DeviceName: token.DeviceName,
                    Browser: token.Browser,
                    OperatingSystem: token.OperatingSystem,
                    IpAddress: token.IpAddress,
                    CreatedOnUtc: token.CreatedOnUtc,
                    LastUsedOnUtc: token.LastUsedOnUtc,
                    ExpiresOnUtc: token.ExpiresOnUtc,
                    IsCurrent:
                        group.Key == _currentUser.SessionId);
            })

            .OrderByDescending(x => x.IsCurrent)
            .ThenByDescending(
                x => x.LastUsedOnUtc ?? x.CreatedOnUtc)
            .ToList();

        return Result.Success<IReadOnlyList<SessionResponse>>(
            response);
    }
}
