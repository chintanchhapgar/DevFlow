using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Services;
using DevFlow.Identity.Domain.Authentication;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.RefreshToken;

/// <summary>
/// Handles refresh token exchange.
/// Validates the existing refresh token, issues new access + refresh tokens (rotation).
/// </summary>
internal sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IClock _clock;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IIdentityUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        IClock clock)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _clock = clock;
    }

    public async Task<Result<RefreshTokenResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            UserId.From(request.UserId),
            cancellationToken);

        if (user is null)
            return UserErrors.NotFound;

        // Validate refresh token (domain logic)
        var validationResult = user.ValidateRefreshToken(
            request.RefreshToken,
            _clock.UtcNow);

        if (validationResult.IsFailure)
            return validationResult.Error;

        // Rotate tokens
        var newAccessToken = _jwtProvider.GenerateToken(user);
        var newRefreshToken = _refreshTokenGenerator.Generate();
        var expiresAt = _clock.UtcNow.Add(_refreshTokenGenerator.Expiration);

        user.SetRefreshToken(newRefreshToken, expiresAt);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse(
            AccessToken: newAccessToken,
            RefreshToken: newRefreshToken,
            RefreshTokenExpiresAtUtc: expiresAt);
    }
}
