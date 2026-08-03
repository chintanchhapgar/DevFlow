using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Reports.ProjectSummary;

public sealed record GetProjectSummaryQuery(
    Guid ProjectId)
    : IRequest<Result<GetProjectSummaryResponse>>;
