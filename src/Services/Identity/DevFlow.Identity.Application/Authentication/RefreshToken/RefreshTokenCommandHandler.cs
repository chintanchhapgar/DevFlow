using DevFlow.SharedKernel.Common;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Requests;
using DevFlow.Identity.Domain.Authentication.RefreshTokens;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;

namespace DevFlow.Identity.Application.Authentication.RefreshToken;

internal sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ICurrentRequestInfo _currentRequestInfo;
    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        ICurrentRequestInfo currentRequestInfo)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _currentRequestInfo = currentRequestInfo;
    }

    public async Task<Result<RefreshTokenResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(
            request.RefreshToken,
            cancellationToken);

        if (refreshToken is null)
        {
            return Result.Failure<RefreshTokenResponse>(
                UserErrors.InvalidRefreshToken);
        }

        if (!refreshToken.IsActive)
        {
            // Was this token rotated?
            if (refreshToken.Status == RefreshTokenStatus.Revoked &&
                !string.IsNullOrWhiteSpace(refreshToken.ReplacedByToken))
            {
                var activeTokens =
                    await _refreshTokenRepository.GetActiveByUserIdAsync(
                        refreshToken.UserId,
                        cancellationToken);

                foreach (var token in activeTokens)
                {
                    token.Revoke(
                        reason: "Refresh token reuse detected");
                }

                await _refreshTokenRepository.UpdateRangeAsync(
                    activeTokens,
                    cancellationToken);
            }

            return Result.Failure<RefreshTokenResponse>(
                UserErrors.InvalidRefreshToken);
        }

        var user = await _userRepository.GetByIdAsync(
            refreshToken.UserId,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<RefreshTokenResponse>(
                UserErrors.NotFound);
        }

        var newRefreshTokenValue =
            _refreshTokenGenerator.Generate();

        var expiresOnUtc = DateTime.UtcNow.AddDays(30);

        var newRefreshToken = user.CreateRefreshToken(
            newRefreshTokenValue,
            expiresOnUtc,
            refreshToken.SessionId,
            refreshToken.DeviceName,
            refreshToken.Browser,
            refreshToken.OperatingSystem,
            refreshToken.IpAddress,
            refreshToken.UserAgent);

        refreshToken.Revoke(
            newRefreshToken.Token,
            "Refresh token rotated");

        await _refreshTokenRepository.UpdateAsync(
            refreshToken,
            cancellationToken);

        await _refreshTokenRepository.AddAsync(
            newRefreshToken,
            cancellationToken);

        var accessToken =
            _jwtProvider.GenerateAccessToken(
                user,
                refreshToken.SessionId);

        return Result.Success(
            new RefreshTokenResponse(
                accessToken,
                newRefreshToken.Token,
                newRefreshToken.ExpiresOnUtc));
    }
}
