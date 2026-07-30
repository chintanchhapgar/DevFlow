using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.Summary;

public sealed record GetWorklogSummaryQuery(
    Guid WorkItemId)
    : IRequest<Result<GetWorklogSummaryResponse>>;
