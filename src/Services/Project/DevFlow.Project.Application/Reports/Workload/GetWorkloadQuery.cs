using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Reports.Workload;

public sealed record GetWorkloadQuery(
    Guid ProjectId)
    : IRequest<Result<GetWorkloadResponse>>;
