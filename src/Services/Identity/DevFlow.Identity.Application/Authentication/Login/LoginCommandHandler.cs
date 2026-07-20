using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Services;
using DevFlow.Identity.Domain.Authentication;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Login;

/// <summary>
/// Handles user authentication.
/// Verifies credentials, generates JWT and refresh token.
/// </summary>
internal sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IClock _clock;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IIdentityUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        IClock clock)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _clock = clock;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user is null)
            return UserErrors.InvalidCredentials;

        // Check lockout
        if (user.IsCurrentlyLockedOut(_clock.UtcNow))
            return UserErrors.AccountLocked;

        // Verify password
        var passwordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!passwordValid)
        {
            user.RecordFailedLogin();
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return UserErrors.InvalidCredentials;
        }

        // Generate tokens
        var accessToken = _jwtProvider.GenerateToken(user);
        var refreshToken = _refreshTokenGenerator.Generate();
        var expiresAt = _clock.UtcNow.Add(_refreshTokenGenerator.Expiration);

        // Record successful login
        user.RecordLogin(_clock.UtcNow);
        user.SetRefreshToken(refreshToken, expiresAt);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            RefreshTokenExpiresAtUtc: expiresAt,
            UserId: user.Id.Value,
            Email: user.Email,
            FullName: user.FullName);
    }
}
