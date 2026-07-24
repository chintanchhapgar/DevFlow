using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.ResendVerification;

/// <summary>
/// Requests a new email verification token.
/// </summary>
public sealed record ResendVerificationCommand(
    string Email)
    : IRequest<Result<ResendVerificationResponse>>;
