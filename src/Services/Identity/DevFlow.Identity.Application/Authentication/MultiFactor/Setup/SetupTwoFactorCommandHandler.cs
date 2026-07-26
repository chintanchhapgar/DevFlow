using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Setup;

internal sealed class SetupTwoFactorCommandHandler
    : IRequestHandler<
        SetupTwoFactorCommand,
        Result<SetupTwoFactorResponse>>
{
    private readonly IUserRepository _users;
    private readonly ITotpService _totp;
    private readonly ISecurityEventLogger _securityEventLogger;
    public SetupTwoFactorCommandHandler(
        IUserRepository users,
        ITotpService totp,
        ISecurityEventLogger securityEventLogger)
    {
        _users = users;
        _totp = totp;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<SetupTwoFactorResponse>> Handle(
        SetupTwoFactorCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(
            new UserId(request.UserId),
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<SetupTwoFactorResponse>(
                UserErrors.NotFound);
        }

        var secret = _totp.GenerateSecret();

        var result = user.BeginTwoFactorSetup(secret);

        if (result.IsFailure)
        {
            return Result.Failure<SetupTwoFactorResponse>(
                result.Error);
        }

        Console.WriteLine($"Pending: {user.MultiFactor.Pending}");
        Console.WriteLine($"Secret : {user.MultiFactor.Secret}");

        await _users.UpdateAsync(
            user,
            cancellationToken);

        await _securityEventLogger.LogAsync(
            user.Id,
            SecurityEventType.TwoFactorEnabled,
            cancellationToken: cancellationToken);

        var qrUri = _totp.GenerateQrCodeUri(
            "DevFlow",
            user.Email,
            secret);

        var qrImage = _totp.GenerateQrCodeImage(
            "DevFlow",
            user.Email,
            secret);

        return Result.Success(
            new SetupTwoFactorResponse(
                ManualEntryKey: secret,
                QrCodeUri: qrUri,
                QrCodeImage: qrImage));
    }
}
