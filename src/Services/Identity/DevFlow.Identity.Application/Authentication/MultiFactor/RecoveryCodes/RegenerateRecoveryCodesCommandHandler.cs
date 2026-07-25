using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.Identity.Domain.Authentication.Users.Owned;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.RecoveryCodes;

internal sealed class RegenerateRecoveryCodesCommandHandler
    : IRequestHandler<
        RegenerateRecoveryCodesCommand,
        Result<RegenerateRecoveryCodesResponse>>
{
    private readonly IUserRepository _users;
    private readonly ITotpService _totp;
    private readonly IRecoveryCodeGenerator _recoveryCodeGenerator;

    public RegenerateRecoveryCodesCommandHandler(
        IUserRepository users,
        ITotpService totp,
        IRecoveryCodeGenerator recoveryCodeGenerator)
    {
        _users = users;
        _totp = totp;
        _recoveryCodeGenerator = recoveryCodeGenerator;
    }

    public async Task<Result<RegenerateRecoveryCodesResponse>> Handle(
        RegenerateRecoveryCodesCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(
            new UserId(request.UserId),
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<RegenerateRecoveryCodesResponse>(
                UserErrors.NotFound);
        }

        if (!user.IsTwoFactorEnabled)
        {
            return Result.Failure<RegenerateRecoveryCodesResponse>(
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
            return Result.Failure<RegenerateRecoveryCodesResponse>(
                verificationResult.Error);
        }

        var codes = _recoveryCodeGenerator.Generate(10);

        user.ReplaceRecoveryCodes(
            codes.Select(RecoveryCode.Create));

        await _users.UpdateAsync(
            user,
            cancellationToken);

        return Result.Success(
            new RegenerateRecoveryCodesResponse(
                codes));
    }
}
