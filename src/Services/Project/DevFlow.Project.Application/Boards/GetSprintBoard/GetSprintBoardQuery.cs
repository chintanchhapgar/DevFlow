using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Boards.GetSprintBoard;

public sealed record GetSprintBoardQuery(
    Guid SprintId)
    : IRequest<Result<GetSprintBoardResponse>>;
