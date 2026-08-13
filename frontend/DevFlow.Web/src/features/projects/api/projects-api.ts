import { projectApiClient } from "@/lib/api/project-api-client";

/* -------------------------------------------------------------------------- */
/* Project constants                                                          */
/* -------------------------------------------------------------------------- */

export const ProjectVisibility = {
  Private: 1,
  Internal: 2,
  Public: 3,
} as const;

export type ProjectVisibility =
  (typeof ProjectVisibility)[keyof typeof ProjectVisibility];

export const ProjectRole = {
  Owner: 1,
  Administrator: 2,
  Member: 3,
  Guest: 4,
} as const;

export type ProjectRole =
  (typeof ProjectRole)[keyof typeof ProjectRole];

/* -------------------------------------------------------------------------- */
/* Project list                                                               */
/* -------------------------------------------------------------------------- */

export interface GetProjectsParams {
  page?: number;
  pageSize?: number;
  search?: string;
  sortBy?: string;
  sortDirection?: "asc" | "desc";
}

export interface ProjectListItem {
  projectId: string;
  key: string;
  name: string;
  status: string;
  visibility: string;
  ownerId: string;
  memberCount: number;
}

export interface PagedProjects {
  items: ProjectListItem[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

/* -------------------------------------------------------------------------- */
/* Project detail                                                             */
/* -------------------------------------------------------------------------- */

export interface ProjectMember {
  userId: string;
  role: string;
  joinedOnUtc: string;
}

export interface ProjectDetail {
  projectId: string;
  key: string;
  name: string;
  description: string | null;
  status: string;
  visibility: string;
  ownerId: string;
  members: ProjectMember[] | null;
}

/* -------------------------------------------------------------------------- */
/* Create Project                                                             */
/* -------------------------------------------------------------------------- */

export interface CreateProjectRequest {
  key: string;
  name: string;
  description?: string | null;
  visibility: ProjectVisibility;
}

export interface CreateProjectResponse {
  projectId: string;
  key: string;
  name: string;
}

export async function createProject(
  request: CreateProjectRequest,
): Promise<CreateProjectResponse> {
  const response = await projectApiClient.post(
    "/api/projects",
    request,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Get Projects                                                               */
/* -------------------------------------------------------------------------- */

export async function getProjects(
  params: GetProjectsParams = {},
): Promise<PagedProjects> {
  const {
    page = 1,
    pageSize = 20,
    search,
    sortBy,
    sortDirection,
  } = params;

  const response = await projectApiClient.get(
    "/api/projects",
    {
      params: {
        page,
        pageSize,

        ...(search?.trim()
          ? {
              search: search.trim(),
            }
          : {}),

        ...(sortBy
          ? {
              sortBy,
            }
          : {}),

        ...(sortDirection
          ? {
              sortDirection,
            }
          : {}),
      },
    },
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Get Project                                                                */
/* -------------------------------------------------------------------------- */

export async function getProject(
  projectId: string,
): Promise<ProjectDetail> {
  const response = await projectApiClient.get(
    `/api/projects/${projectId}`,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Update Project                                                             */
/* -------------------------------------------------------------------------- */

export interface UpdateProjectRequest {
  name: string;
  description?: string | null;
  visibility: ProjectVisibility;
}

export interface UpdateProjectResponse {
  projectId: string;
  key: string;
  name: string;
}

export async function updateProject(
  projectId: string,
  request: UpdateProjectRequest,
): Promise<UpdateProjectResponse> {
  const response = await projectApiClient.put(
    `/api/projects/${projectId}`,
    request,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Archive Project                                                            */
/* -------------------------------------------------------------------------- */

export interface ProjectLifecycleResponse {
  projectId: string;
  key: string;
  name: string;
  status: string;
}

export async function archiveProject(
  projectId: string,
): Promise<ProjectLifecycleResponse> {
  const response = await projectApiClient.patch(
    `/api/projects/${projectId}/archive`,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Restore Project                                                            */
/* -------------------------------------------------------------------------- */

export async function restoreProject(
  projectId: string,
): Promise<ProjectLifecycleResponse> {
  const response = await projectApiClient.patch(
    `/api/projects/${projectId}/restore`,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Get Project Members                                                        */
/* -------------------------------------------------------------------------- */

export interface GetProjectMemberResponse {
  userId: string;
  role: string;
  joinedOnUtc: string;
}

export async function getProjectMembers(
  projectId: string,
): Promise<GetProjectMemberResponse[]> {
  const response = await projectApiClient.get(
    `/api/projects/${projectId}/members`,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Add Project Member                                                         */
/* -------------------------------------------------------------------------- */

export interface AddProjectMemberRequest {
  userId: string;
  role: ProjectRole;
}

export interface AddProjectMemberResponse {
  projectId: string;
  userId: string;
  role: string;
}

export async function addProjectMember(
  projectId: string,
  request: AddProjectMemberRequest,
): Promise<AddProjectMemberResponse> {
  const response = await projectApiClient.post(
    `/api/projects/${projectId}/members`,
    request,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Update Project Member Role                                                 */
/* -------------------------------------------------------------------------- */

export interface UpdateProjectMemberRoleRequest {
  role: ProjectRole;
}

export interface UpdateProjectMemberRoleResponse {
  projectId: string;
  userId: string;
  role: string;
}

export async function updateProjectMemberRole(
  projectId: string,
  userId: string,
  request: UpdateProjectMemberRoleRequest,
): Promise<UpdateProjectMemberRoleResponse> {
  const response = await projectApiClient.patch(
    `/api/projects/${projectId}/members/${userId}/role`,
    request,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Remove Project Member                                                      */
/* -------------------------------------------------------------------------- */

export interface RemoveProjectMemberResponse {
  projectId: string;
  userId: string;
}

export async function removeProjectMember(
  projectId: string,
  userId: string,
): Promise<RemoveProjectMemberResponse> {
  const response = await projectApiClient.delete(
    `/api/projects/${projectId}/members/${userId}`,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Invite Project Member                                                      */
/* -------------------------------------------------------------------------- */

export interface InviteProjectMemberRequest {
  email: string;
  role: ProjectRole;
}

export interface InviteProjectMemberResponse {
  invitationId: string;
  projectId: string;
  email: string;
  role: string;
  token: string;
}

export async function inviteProjectMember(
  projectId: string,
  request: InviteProjectMemberRequest,
): Promise<InviteProjectMemberResponse> {
  const response = await projectApiClient.post(
    `/api/projects/${projectId}/invitations`,
    request,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Get Project Invitations                                                    */
/* -------------------------------------------------------------------------- */

export interface ProjectInvitation {
  invitationId: string;
  email: string;
  role: string;
  status: string;
  token: string;
  invitedBy: string;
  invitedOnUtc: string;
  expiresOnUtc: string;
  acceptedOnUtc: string | null;
}

export async function getProjectInvitations(
  projectId: string,
): Promise<ProjectInvitation[]> {
  const response = await projectApiClient.get(
    `/api/projects/${projectId}/invitations`,
  );

  return response.data.data;
}

/* -------------------------------------------------------------------------- */
/* Revoke Project Invitation                                                  */
/* -------------------------------------------------------------------------- */

export interface RevokeProjectInvitationResponse {
  projectId: string;
  invitationId: string;
  status: string;
}

export async function revokeProjectInvitation(
  projectId: string,
  invitationId: string,
): Promise<RevokeProjectInvitationResponse> {
  const response = await projectApiClient.delete(
    `/api/projects/${projectId}/invitations/${invitationId}`,
  );

  return response.data.data;
}

export interface InvitationActionResponse {
  projectId: string;
  invitationId: string;
  status: string;
}

export async function acceptProjectInvitation(
  token: string,
): Promise<InvitationActionResponse> {
  const response = await projectApiClient.post(
    "/api/projects/invitations/accept",
    { token },
  );

  return response.data.data;
}

export async function declineProjectInvitation(
  token: string,
): Promise<InvitationActionResponse> {
  const response = await projectApiClient.post(
    "/api/projects/invitations/decline",
    { token },
  );

  return response.data.data;
}