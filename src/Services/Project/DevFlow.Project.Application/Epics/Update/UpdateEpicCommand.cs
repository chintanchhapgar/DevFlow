using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.Update;

public sealed record UpdateEpicCommand(
    Guid EpicId,
    string Name,
    string? Description,
    string Color,
    DateTime? StartDate,
    DateTime? DueDate)
    : IRequest<Result<UpdateEpicResponse>>;
