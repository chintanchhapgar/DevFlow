using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.Delete;

public sealed record DeleteWorklogCommand(
    Guid WorklogId)
    : IRequest<Result<DeleteWorklogResponse>>;
