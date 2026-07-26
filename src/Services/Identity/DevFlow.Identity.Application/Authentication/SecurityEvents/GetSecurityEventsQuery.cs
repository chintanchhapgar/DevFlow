using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.SecurityEvents;

public sealed record GetSecurityEventsQuery()
    : IRequest<Result<IReadOnlyList<SecurityEventResponse>>>;
