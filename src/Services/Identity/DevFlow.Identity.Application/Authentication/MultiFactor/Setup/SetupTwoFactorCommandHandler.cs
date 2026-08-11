using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
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
    private readonly ICurrentUser _currentUser;

    public SetupTwoFactorCommandHandler(
        IUserRepository users,
        ITotpService totp,
        ICurrentUser currentUser)
    {
        _users = users;
        _totp = totp;
        _currentUser = currentUser;
    }

    public async Task<Result<SetupTwoFactorResponse>> Handle(
        SetupTwoFactorCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(
            new UserId(_currentUser.UserId),
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

        // NEVER log the TOTP secret.

        await _users.UpdateAsync(
            user,
            cancellationToken);

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
