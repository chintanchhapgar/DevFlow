namespace DevFlow.Project.Application.Reports.Burndown;

public sealed record BurndownPointResponse(
    DateOnly Date,
    int Remaining,
    int Completed,
    int Ideal);
