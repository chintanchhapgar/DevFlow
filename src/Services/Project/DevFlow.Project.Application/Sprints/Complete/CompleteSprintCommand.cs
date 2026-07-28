using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Complete;

public sealed record CompleteSprintCommand(
    Guid SprintId)
    : IRequest<Result<CompleteSprintResponse>>;
