using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.Subtasks.Create;

public sealed record CreateSubtaskCommand(
    Guid ParentId,
    string Title,
    string? Description,
    WorkItemPriority Priority)
    : IRequest<Result<CreateSubtaskResponse>>;
