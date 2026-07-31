using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Dashboard.GetDashboard;

public sealed record GetDashboardQuery(
    Guid ProjectId)
    : IRequest<Result<GetDashboardResponse>>;
