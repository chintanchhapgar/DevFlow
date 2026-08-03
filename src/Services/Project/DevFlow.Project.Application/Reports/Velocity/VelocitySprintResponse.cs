namespace DevFlow.Project.Application.Reports.Velocity;

public sealed record VelocitySprintResponse(
    Guid SprintId,
    string SprintName,
    int Committed,
    int Completed);
