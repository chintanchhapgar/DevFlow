using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Verify;

public sealed record VerifyTwoFactorCommand(
    string Code)
    : IRequest<Result<VerifyTwoFactorResponse>>;
