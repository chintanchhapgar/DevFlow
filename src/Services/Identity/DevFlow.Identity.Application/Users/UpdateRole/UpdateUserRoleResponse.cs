namespace DevFlow.Identity.Application.Users.UpdateRole;

public sealed record UpdateUserRoleResponse(
    Guid UserId,
    string Role);
