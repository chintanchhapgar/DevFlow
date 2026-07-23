using DevFlow.Authentication.Users;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Login;

/// <summary>
/// Handles user login.
/// </summary>
internal sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenGenerator refreshTokenGenerator
        )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<LoginResponse>(
                UserErrors.InvalidCredentials);
        }

        if (!_passwordHasher.Verify(
                request.Password,
                user.PasswordHash))
        {
            return Result.Failure<LoginResponse>(
                UserErrors.InvalidCredentials);
        }

        if (!user.IsActive)
        {
            return Result.Failure<LoginResponse>(
                UserErrors.UserInactive);
        }

        var refreshTokenValue =
            _refreshTokenGenerator.Generate();

        var refreshToken = user.CreateRefreshToken(
            refreshTokenValue,
            DateTime.UtcNow.AddDays(30));

        await _refreshTokenRepository.AddAsync(
            refreshToken,
            cancellationToken);

        var accessToken =
            _jwtProvider.GenerateAccessToken(user);

        return new LoginResponse(
            accessToken,
            refreshToken.Token,
            refreshToken.ExpiresOnUtc);
    }
}
