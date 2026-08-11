using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.RecoveryCodes;

/// <summary>
/// Regenerates recovery codes for a user.
/// Requires a valid TOTP or recovery code.
/// </summary>
public sealed record RegenerateRecoveryCodesCommand(
    string Code,
    bool IsRecoveryCode)
    : IRequest<Result<RegenerateRecoveryCodesResponse>>;
