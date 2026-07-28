using DevFlow.Project.Domain.Sprints.Enums;

namespace DevFlow.Project.Application.Sprints.GetAll;

public sealed record SprintItemResponse(
    Guid SprintId,
    string Name,
    string? Goal,
    SprintStatus Status,
    DateOnly StartDate,
    DateOnly EndDate);

public sealed record GetAllSprintsResponse(
    IReadOnlyList<SprintItemResponse> Items,
    int TotalCount,
    int Page,
    int PageSize);
