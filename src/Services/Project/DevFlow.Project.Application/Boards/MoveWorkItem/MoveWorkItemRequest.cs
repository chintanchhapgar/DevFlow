using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.Boards.MoveWorkItem;

public sealed record MoveWorkItemRequest(
    WorkItemStatus Status);
