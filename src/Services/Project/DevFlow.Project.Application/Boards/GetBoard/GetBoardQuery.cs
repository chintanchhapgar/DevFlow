using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Boards.GetBoard;

public sealed record GetBoardQuery(
    Guid ProjectId)
    : IRequest<Result<GetBoardResponse>>;
