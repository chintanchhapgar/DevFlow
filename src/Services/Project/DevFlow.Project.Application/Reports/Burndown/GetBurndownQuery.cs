using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Reports.Burndown;

public sealed record GetBurndownQuery(
    Guid SprintId)
    : IRequest<Result<GetBurndownResponse>>;
