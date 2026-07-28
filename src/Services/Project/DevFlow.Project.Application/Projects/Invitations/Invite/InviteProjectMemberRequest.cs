using DevFlow.Project.Domain.Projects.Enums;

namespace DevFlow.Project.Application.Projects.Invitations.Invite;

public sealed record InviteProjectMemberRequest(
    string Email,
    ProjectRole Role);
