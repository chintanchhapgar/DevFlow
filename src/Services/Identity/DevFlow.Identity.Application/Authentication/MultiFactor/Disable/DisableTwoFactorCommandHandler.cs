using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Disable;

internal sealed class DisableTwoFactorCommandHandler
    : IRequestHandler<
        DisableTwoFactorCommand,
        Result<DisableTwoFactorResponse>>
{
    private readonly IUserRepository _users;
    private readonly ITotpService _totp;
    private readonly ISecurityEventLogger _securityEventLogger;
    public DisableTwoFactorCommandHandler(
        IUserRepository users,
        ITotpService totp,
        ISecurityEventLogger securityEventLogger)
    {
        _users = users;
        _totp = totp;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<DisableTwoFactorResponse>> Handle(
        DisableTwoFactorCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(
            new UserId(request.UserId),
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<DisableTwoFactorResponse>(
                UserErrors.NotFound);
        }

        if (!user.IsTwoFactorEnabled)
        {
            return Result.Failure<DisableTwoFactorResponse>(
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
            return Result.Failure<DisableTwoFactorResponse>(
                verificationResult.Error);
        }

        var result = user.DisableTwoFactor();

        if (result.IsFailure)
        {
            return Result.Failure<DisableTwoFactorResponse>(
               verificationResult.Error);
        }

        await _users.UpdateAsync(
            user,
            cancellationToken);

        await _securityEventLogger.LogAsync(
            user.Id,
            SecurityEventType.TwoFactorDisabled,
            cancellationToken: cancellationToken);

        return Result.Success(
             new DisableTwoFactorResponse());
        }
}
