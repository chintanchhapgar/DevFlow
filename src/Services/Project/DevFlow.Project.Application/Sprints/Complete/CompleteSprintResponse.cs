using DevFlow.Project.Domain.Sprints.Enums;

namespace DevFlow.Project.Application.Sprints.Complete;

public sealed record CompleteSprintResponse(
    Guid SprintId,
    SprintStatus Status,
    DateTime CompletedOnUtc);
