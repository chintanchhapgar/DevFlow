using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.Identity.Domain.Authentication.Users.Owned;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Verify;

internal sealed class VerifyTwoFactorCommandHandler
    : IRequestHandler<
        VerifyTwoFactorCommand,
        Result<VerifyTwoFactorResponse>>
{
    private readonly IUserRepository _users;
    private readonly ITotpService _totp;
    private readonly IRecoveryCodeGenerator _recoveryCodes;
    private readonly ICurrentUser _currentUser;
    private readonly ISecurityEventLogger _securityEventLogger;

    public VerifyTwoFactorCommandHandler(
        IUserRepository users,
        ITotpService totp,
        IRecoveryCodeGenerator recoveryCodes,
        ICurrentUser currentUser,
        ISecurityEventLogger securityEventLogger)
    {
        _users = users;
        _totp = totp;
        _recoveryCodes = recoveryCodes;
        _currentUser = currentUser;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<VerifyTwoFactorResponse>> Handle(
        VerifyTwoFactorCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(
            new UserId(_currentUser.UserId),
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<VerifyTwoFactorResponse>(
                UserErrors.NotFound);
        }

        if (!user.IsTwoFactorSetupPending)
        {
            return Result.Failure<VerifyTwoFactorResponse>(
                MultiFactorErrors.NotPending);
        }

        var verifyResult = _totp.VerifyCode(
            user.TwoFactorSecret!,
            request.Code);

        if (verifyResult.IsFailure)
        {
            return Result.Failure<VerifyTwoFactorResponse>(
                verifyResult.Error);
        }

        var enableResult = user.CompleteTwoFactorSetup();

        if (enableResult.IsFailure)
        {
            return Result.Failure<VerifyTwoFactorResponse>(
                enableResult.Error);
        }

        var recoveryCodes = _recoveryCodes.Generate();

        user.ReplaceRecoveryCodes(
            recoveryCodes.Select(RecoveryCode.Create));

        await _users.UpdateAsync(
            user,
            cancellationToken);

        await _securityEventLogger.LogAsync(
            user.Id,
            SecurityEventType.TwoFactorEnabled,
            cancellationToken: cancellationToken);

        return Result.Success(
            new VerifyTwoFactorResponse(
                recoveryCodes));
    }
}
