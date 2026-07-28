using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Create;

public sealed record CreateSprintCommand(
    Guid ProjectId,
    string Name,
    string? Goal,
    DateOnly StartDate,
    DateOnly EndDate)
    : IRequest<Result<CreateSprintResponse>>;
