using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.VerifyEmail;

/// <summary>
/// Verifies a user's email address.
/// </summary>
public sealed record VerifyEmailCommand(
    string Token)
    : IRequest<Result<VerifyEmailResponse>>;
