namespace DevFlow.Project.Application.Projects.Invitations.Accept;

public sealed record AcceptProjectInvitationResponse(
    Guid ProjectId,
    Guid InvitationId,
    Guid UserId,
    string Role);
