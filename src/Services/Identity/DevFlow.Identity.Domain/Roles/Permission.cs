namespace DevFlow.Identity.Domain.Roles;

/// <summary>
/// Fine-grained permissions for feature-level authorization.
/// </summary>
public enum Permissions
{
    // User management
    ReadUsers = 1,
    ManageUsers = 2,

    // Project management
    ReadProjects = 10,
    CreateProjects = 11,
    ManageProjects = 12,
    DeleteProjects = 13,

    // Work items
    ReadWorkItems = 20,
    CreateWorkItems = 21,
    ManageWorkItems = 22,
    DeleteWorkItems = 23
}
