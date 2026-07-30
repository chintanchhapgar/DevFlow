using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.Update;

public sealed record UpdateWorklogCommand(
    Guid WorklogId,
    string? Description,
    DateTime StartedAtUtc,
    DateTime EndedAtUtc)
    : IRequest<Result<UpdateWorklogResponse>>;
