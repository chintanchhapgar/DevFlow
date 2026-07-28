using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.GetById;

public sealed record GetSprintByIdQuery(
    Guid SprintId)
    : IRequest<Result<GetSprintByIdResponse>>;
