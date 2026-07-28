using DevFlow.Project.Domain.Sprints.Enums;

namespace DevFlow.Project.Application.Sprints.GetById;

public sealed record GetSprintByIdResponse(
    Guid SprintId,
    Guid ProjectId,
    string Name,
    string? Goal,
    SprintStatus Status,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTime? StartedOnUtc,
    DateTime? CompletedOnUtc,
    DateTime CreatedOnUtc);
