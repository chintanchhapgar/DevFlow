using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.GetAll;

public sealed record GetAllEpicsQuery(
    Guid ProjectId)
    : IRequest<Result<IReadOnlyList<GetAllEpicsResponse>>>;
