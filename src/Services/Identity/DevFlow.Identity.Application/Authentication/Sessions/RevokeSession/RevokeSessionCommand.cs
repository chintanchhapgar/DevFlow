using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeSession;

public sealed record RevokeSessionCommand(
    Guid SessionId)
    : IRequest<Result<RevokeSessionResponse>>;
