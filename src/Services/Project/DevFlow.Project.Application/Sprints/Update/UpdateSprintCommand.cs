using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Update;

public sealed record UpdateSprintCommand(
    Guid SprintId,
    string Name,
    string? Goal,
    DateOnly StartDate,
    DateOnly EndDate)
    : IRequest<Result<UpdateSprintResponse>>;