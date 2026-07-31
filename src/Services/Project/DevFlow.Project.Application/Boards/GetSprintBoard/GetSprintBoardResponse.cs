namespace DevFlow.Project.Application.Boards.GetSprintBoard;

public sealed record GetSprintBoardResponse(
    Guid SprintId,
    string SprintName,
    IReadOnlyList<SprintBoardColumnResponse> Columns);
