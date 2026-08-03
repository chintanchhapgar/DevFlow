using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Reports.Velocity;

public sealed record GetVelocityQuery(
    Guid ProjectId)
    : IRequest<Result<GetVelocityResponse>>;
