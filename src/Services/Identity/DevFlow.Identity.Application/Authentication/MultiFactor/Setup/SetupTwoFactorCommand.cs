using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Setup;

public sealed record SetupTwoFactorCommand(
    Guid UserId)
    : IRequest<Result<SetupTwoFactorResponse>>;
