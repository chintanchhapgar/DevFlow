using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.Subtasks.Create;

public sealed record CreateSubtaskRequest(
    string Title,
    string? Description,
    WorkItemPriority Priority);
