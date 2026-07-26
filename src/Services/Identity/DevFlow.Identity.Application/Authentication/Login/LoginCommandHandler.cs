using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.SharedKernel.Common;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Requests;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;

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
    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenGenerator refreshTokenGenerator,
        ICurrentRequestInfo currentRequestInfo
        )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenGenerator = refreshTokenGenerator;
        _currentRequestInfo = currentRequestInfo;
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

        if (!_passwordHasher.Verify(
                request.Password,
                user.PasswordHash))
        {
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
