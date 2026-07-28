using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Delete;

public sealed record DeleteSprintCommand(
    Guid SprintId)
    : IRequest<Result<DeleteSprintResponse>>;
