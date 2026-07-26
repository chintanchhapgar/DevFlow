
using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.ResetPassword;

internal sealed class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResponse>>
{
    private readonly IPasswordResetTokenRepository _passwordResetRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISecurityEventLogger _securityEventLogger;
    public ResetPasswordCommandHandler(
        IPasswordResetTokenRepository passwordResetRepository,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ISecurityEventLogger securityEventLogger)
    {
        _passwordResetRepository = passwordResetRepository;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<ResetPasswordResponse>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var resetToken =
            await _passwordResetRepository.GetByTokenAsync(
                request.Token,
                cancellationToken);

        if (resetToken is null || !resetToken.IsActive)
        {
            return Result.Failure<ResetPasswordResponse>(
                UserErrors.InvalidResetToken);
        }

        var user = await _userRepository.GetByIdAsync(
            resetToken.UserId,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<ResetPasswordResponse>(
                UserErrors.UserNotFound);
        }

        user.ChangePassword(
            _passwordHasher.Hash(request.NewPassword));

        resetToken.MarkAsUsed();

        await _passwordResetRepository.UpdateAsync(
            resetToken,
            cancellationToken);

        var refreshTokens =
            await _refreshTokenRepository.GetActiveByUserIdAsync(
                user.Id,
                cancellationToken);

        foreach (var token in refreshTokens)
        {
            token.Revoke();

            await _refreshTokenRepository.UpdateAsync(
                token,
                cancellationToken);
        }

        await _securityEventLogger.LogAsync(
            user.Id,
            SecurityEventType.PasswordReset,
            cancellationToken: cancellationToken);

        return new ResetPasswordResponse();
    }
}
