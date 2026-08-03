namespace DevFlow.Project.Application.Reports.Velocity;

public sealed record GetVelocityResponse(
    Guid ProjectId,
    IReadOnlyList<VelocitySprintResponse> Sprints);
