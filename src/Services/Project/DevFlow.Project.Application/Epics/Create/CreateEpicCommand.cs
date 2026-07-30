using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.Create;

public sealed record CreateEpicCommand(
    Guid ProjectId,
    string Name,
    string? Description,
    string Color,
    DateTime? StartDate,
    DateTime? DueDate)
    : IRequest<Result<CreateEpicResponse>>;
