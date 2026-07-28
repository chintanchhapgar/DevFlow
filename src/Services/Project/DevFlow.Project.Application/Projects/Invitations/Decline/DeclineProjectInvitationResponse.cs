namespace DevFlow.Project.Application.Projects.Invitations.Decline;

public sealed record DeclineProjectInvitationResponse(
    Guid ProjectId,
    Guid InvitationId,
    string Status);
