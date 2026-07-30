using DevFlow.Project.Domain.Sprints.Enums;

namespace DevFlow.Project.Application.Sprints.GetAll;

public sealed record SprintListItemResponse(
    Guid SprintId,
    string Name,
    string? Goal,
    SprintStatus Status,
    DateOnly StartDate,
    DateOnly EndDate);
