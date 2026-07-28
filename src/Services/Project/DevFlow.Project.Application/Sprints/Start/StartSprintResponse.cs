using DevFlow.Project.Domain.Sprints.Enums;

namespace DevFlow.Project.Application.Sprints.Start;

public sealed record StartSprintResponse(
    Guid SprintId,
    SprintStatus Status,
    DateTime StartedOnUtc);
