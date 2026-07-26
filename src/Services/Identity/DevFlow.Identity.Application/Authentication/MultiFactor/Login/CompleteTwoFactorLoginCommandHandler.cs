using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.SharedKernel.Common;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Requests;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;
using System.Linq;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;

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
    private readonly ICurrentRequestInfo _currentRequestInfo;
    public CompleteTwoFactorLoginCommandHandler(
        IUserRepository users,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITotpService totp,
        IRefreshTokenRepository refreshTokenRepository,
        ICurrentRequestInfo currentRequestInfo)
    {
        _users = users;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _totp = totp;
        _refreshTokenRepository = refreshTokenRepository;
        _currentRequestInfo = currentRequestInfo;
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
      

        var refreshTokenValue =
            _refreshTokenGenerator.Generate();

        var expiresOnUtc =
            DateTime.UtcNow.AddDays(30);

        var sessionId = Guid.NewGuid();
        var refreshToken = user.CreateRefreshToken(
            refreshTokenValue,
            expiresOnUtc,
            sessionId,
            _currentRequestInfo.DeviceName,
            _currentRequestInfo.Browser,
            _currentRequestInfo.OperatingSystem,
            _currentRequestInfo.IpAddress,
            _currentRequestInfo.UserAgent);


        var accessToken =
          _jwtProvider.GenerateAccessToken(
              user,
              sessionId);

        await _refreshTokenRepository.AddAsync(
            refreshToken,
            cancellationToken);

        return AuthenticationResponse.Success(
            accessToken,
            refreshToken.Token,
            refreshToken.ExpiresOnUtc);
    }
}
