using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.GetById;

public sealed record GetEpicByIdQuery(
    Guid EpicId)
    : IRequest<Result<GetEpicByIdResponse>>;
