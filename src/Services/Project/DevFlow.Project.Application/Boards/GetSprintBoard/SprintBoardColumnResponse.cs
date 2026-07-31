using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.Boards.GetSprintBoard;

public sealed record SprintBoardColumnResponse(
    WorkItemStatus Status,
    IReadOnlyList<SprintBoardWorkItemResponse> Items);
