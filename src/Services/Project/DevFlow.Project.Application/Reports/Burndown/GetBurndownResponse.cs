namespace DevFlow.Project.Application.Reports.Burndown;

public sealed record GetBurndownResponse(
    Guid SprintId,
    string SprintName,
    IReadOnlyList<BurndownPointResponse> Points);
