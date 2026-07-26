using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeAllSessions;

public sealed record RevokeAllSessionsCommand()
    : IRequest<Result<RevokeAllSessionsResponse>>;
