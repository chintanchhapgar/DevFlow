using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeOtherSessions;

public sealed record RevokeOtherSessionsCommand
    : IRequest<Result<RevokeOtherSessionsResponse>>;
