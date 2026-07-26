using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Sessions.GetSessions;

public sealed record GetSessionsQuery
    : IRequest<Result<IReadOnlyList<SessionResponse>>>;
