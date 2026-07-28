namespace DevFlow.Project.Application.Projects.Invitations.GetAll;

public sealed record GetProjectInvitationsResponse(
    Guid InvitationId,
    string Email,
    string Role,
    string Status,
    Guid Token,
    Guid InvitedBy,
    DateTime InvitedOnUtc,
    DateTime ExpiresOnUtc,
    DateTime? AcceptedOnUtc);
