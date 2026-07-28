namespace DevFlow.Project.Application.Projects.Invitations.Revoke;

public sealed record RevokeProjectInvitationResponse(
    Guid ProjectId,
    Guid InvitationId,
    string Status);
