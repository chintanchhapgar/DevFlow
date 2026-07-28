namespace DevFlow.Project.Application.Projects.Invitations.Invite;

public sealed record InviteProjectMemberResponse(
    Guid InvitationId,
    Guid ProjectId,
    string Email,
    string Role,
    Guid Token);
