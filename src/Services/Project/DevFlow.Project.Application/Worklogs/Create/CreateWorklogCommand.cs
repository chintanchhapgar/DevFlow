using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.Create;

public sealed record CreateWorklogCommand(
    Guid WorkItemId,
    string? Description,
    DateTime StartedAtUtc,
    DateTime EndedAtUtc)
    : IRequest<Result<CreateWorklogResponse>>;
