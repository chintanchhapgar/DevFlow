using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Logout;

internal sealed class LogoutCommandHandler
    : IRequestHandler<LogoutCommand, Result<LogoutResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
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

        return new LogoutResponse();
    }
}
