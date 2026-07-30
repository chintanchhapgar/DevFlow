using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.Delete;

public sealed record DeleteEpicCommand(
    Guid EpicId)
    : IRequest<Result<DeleteEpicResponse>>;
