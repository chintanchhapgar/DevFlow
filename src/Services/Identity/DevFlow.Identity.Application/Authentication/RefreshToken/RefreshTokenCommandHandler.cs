using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.RefreshTokens;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.RefreshToken;

internal sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
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

        var newRefreshToken = user.CreateRefreshToken(
            newRefreshTokenValue,
            DateTime.UtcNow.AddDays(30));

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
            _jwtProvider.GenerateAccessToken(user);

        return Result.Success(
            new RefreshTokenResponse(
                accessToken,
                newRefreshToken.Token,
                newRefreshToken.ExpiresOnUtc));
    }
}
