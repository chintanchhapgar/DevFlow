using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Disable;

/// <summary>
/// Disables two-factor authentication.
/// The user must provide either a valid TOTP code
/// or a valid recovery code.
/// </summary>
public sealed record DisableTwoFactorCommand(
    Guid UserId,
    string Code,
    bool IsRecoveryCode)
    : IRequest<Result<DisableTwoFactorResponse>>;
