using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Login;

/// <summary>
/// Completes an MFA login after username/password
/// authentication has already succeeded.
/// </summary>
public sealed record CompleteTwoFactorLoginCommand(
    Guid UserId,
    string Code,
    bool IsRecoveryCode = false)
    : IRequest<Result<AuthenticationResponse>>;
