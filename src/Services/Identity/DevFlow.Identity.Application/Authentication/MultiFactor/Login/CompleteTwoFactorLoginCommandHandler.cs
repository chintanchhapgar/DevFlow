using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;
using System.Linq;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Login;

internal sealed class CompleteTwoFactorLoginCommandHandler
    : IRequestHandler<
        CompleteTwoFactorLoginCommand,
        Result<AuthenticationResponse>>
{
    private readonly IUserRepository _users;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITotpService _totp;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    public CompleteTwoFactorLoginCommandHandler(
        IUserRepository users,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITotpService totp,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _users = users;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _totp = totp;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<AuthenticationResponse>> Handle(
        CompleteTwoFactorLoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(
            new UserId(request.UserId),
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<AuthenticationResponse>(
                UserErrors.NotFound);
        }

        if (!user.IsTwoFactorEnabled)
        {
            return Result.Failure<AuthenticationResponse>(
                MultiFactorErrors.NotEnabled);
        }

        Result verificationResult;

        if (request.IsRecoveryCode)
        {
            verificationResult =
                user.TryUseRecoveryCode(request.Code);
        }
        else
        {
            verificationResult =
                _totp.VerifyCode(
                    user.TwoFactorSecret!,
                    request.Code);
        }

        if (verificationResult.IsFailure)
        {
            return Result.Failure<AuthenticationResponse>(
                verificationResult.Error);
        }

        var accessToken =
    _jwtProvider.GenerateAccessToken(user);

        var refreshTokenValue =
            _refreshTokenGenerator.Generate();

        var expiresOnUtc =
            DateTime.UtcNow.AddDays(30);

        var refreshToken = user.CreateRefreshToken(
            refreshTokenValue,
            expiresOnUtc);

        await _refreshTokenRepository.AddAsync(
            refreshToken,
            cancellationToken);

        return AuthenticationResponse.Success(
            accessToken,
            refreshToken.Token,
            refreshToken.ExpiresOnUtc);
    }
}
