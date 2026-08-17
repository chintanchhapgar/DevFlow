export const UserRole = {
  Member: "Member",
  ProjectManager: "ProjectManager",
  Administrator: "Administrator",
  SystemAdministrator: "SystemAdministrator",
} as const;

export type UserRole =
  (typeof UserRole)[keyof typeof UserRole];

export function isMember(role: string | undefined): boolean {
  return role === UserRole.Member;
}

export function canCreateProjects(
  role: string | undefined,
): boolean {
  return role === UserRole.ProjectManager ||
    role === UserRole.Administrator ||
    role === UserRole.SystemAdministrator;
}

export function canViewReports(
  role: string | undefined,
): boolean {
  return role === UserRole.ProjectManager ||
    role === UserRole.Administrator ||
    role === UserRole.SystemAdministrator;
}

export function canManageUsers(
  role: string | undefined,
): boolean {
  return role === UserRole.Administrator ||
    role === UserRole.SystemAdministrator;
}
