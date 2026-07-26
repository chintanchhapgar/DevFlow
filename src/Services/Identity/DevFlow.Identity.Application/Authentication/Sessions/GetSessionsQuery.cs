using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Sessions;

public sealed record GetSessionsQuery
    : IRequest<Result<IReadOnlyList<SessionResponse>>>;
