using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.GetAll;

public sealed record GetAllLabelsQuery(
    Guid ProjectId)
    : IRequest<Result<IReadOnlyList<GetAllLabelsResponse>>>;
