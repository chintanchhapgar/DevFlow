using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Start;

public sealed record StartSprintCommand(
    Guid SprintId)
    : IRequest<Result<StartSprintResponse>>;
