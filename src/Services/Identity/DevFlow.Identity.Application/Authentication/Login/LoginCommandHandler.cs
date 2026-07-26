using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Options;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Requests;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;
using Microsoft.Extensions.Options;


namespace DevFlow.Identity.Application.Authentication.Login;

/// <summary>
/// Handles user login.
/// </summary>
internal sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<AuthenticationResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ICurrentRequestInfo _currentRequestInfo;
    private readonly LockoutOptions _lockoutOptions;
    private readonly ISecurityEventLogger _securityEventLogger;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenGenerator refreshTokenGenerator,
        ICurrentRequestInfo currentRequestInfo,
        IOptions<LockoutOptions> lockoutOptions,
        ISecurityEventLogger securityEventLogger
        )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenGenerator = refreshTokenGenerator;
        _currentRequestInfo = currentRequestInfo;
        _lockoutOptions = lockoutOptions.Value;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<AuthenticationResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<AuthenticationResponse>(
                UserErrors.InvalidCredentials);
        }

        if (user.IsLockedOut)
        {

            await _securityEventLogger.LogAsync(
                user.Id,
                SecurityEventType.AccountLocked,
                cancellationToken: cancellationToken);

            return Result.Failure<AuthenticationResponse>(
                UserErrors.AccountLocked);
        }

        if (!_passwordHasher.Verify(
            request.Password,
            user.PasswordHash))
        {
            user.RecordFailedLogin(
                _lockoutOptions.MaxFailedAttempts,
                TimeSpan.FromMinutes(_lockoutOptions.DurationMinutes));

            await _userRepository.UpdateAsync(
                user,
                cancellationToken);

            await _securityEventLogger.LogAsync(
                user.Id,
                SecurityEventType.LoginFailed,
                "Invalid password",
                cancellationToken);

            return Result.Failure<AuthenticationResponse>(
                UserErrors.InvalidCredentials);
        }

        if (!user.IsActive)
        {
            return Result.Failure<AuthenticationResponse>(
                UserErrors.UserInactive);
        }

        if (user.IsTwoFactorEnabled)
        {
            return AuthenticationResponse.Challenge(
                user.Id.Value);
        }

        if (user.AccessFailedCount > 0 || user.LockoutEndUtc is not null)
        {
            user.ResetFailedLogin();

            await _userRepository.UpdateAsync(
                user,
                cancellationToken);
        }

        var refreshTokenValue =
            _refreshTokenGenerator.Generate();

        var sessionId = Guid.NewGuid();

        var refreshToken = user.CreateRefreshToken(
            refreshTokenValue,
            DateTime.UtcNow.AddDays(30),
            sessionId,
            _currentRequestInfo.DeviceName,
            _currentRequestInfo.Browser,
            _currentRequestInfo.OperatingSystem,
            _currentRequestInfo.IpAddress,
            _currentRequestInfo.UserAgent);

        await _refreshTokenRepository.AddAsync(
            refreshToken,
            cancellationToken);

        await _securityEventLogger.LogAsync(
            user.Id,
            SecurityEventType.LoginSucceeded,
            cancellationToken: cancellationToken);

        var accessToken =
            _jwtProvider.GenerateAccessToken(
                user,
                sessionId);

        return AuthenticationResponse.Success(
            accessToken,
            refreshToken.Token,
            refreshToken.ExpiresOnUtc);
    }
}
