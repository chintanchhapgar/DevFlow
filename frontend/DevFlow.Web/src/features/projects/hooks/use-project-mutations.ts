import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import {
  addProjectMember,
  archiveProject,
  createProject,
  inviteProjectMember,
  removeProjectMember,
  restoreProject,
  revokeProjectInvitation,
  updateProject,
  updateProjectMemberRole,
  acceptProjectInvitation,
  declineProjectInvitation,
  type AddProjectMemberRequest,
  type CreateProjectRequest,
  type InviteProjectMemberRequest,
  type UpdateProjectMemberRoleRequest,
  type UpdateProjectRequest,
} from "../api/projects-api";

import {
  moveWorkItemToSprint,
} from "../api/project-resources-api";

import { projectKeys } from "./use-project";

/* -------------------------------------------------------------------------- */
/* Create Project                                                             */
/* -------------------------------------------------------------------------- */

export function useCreateProject() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (
      request: CreateProjectRequest,
    ) => createProject(request),

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: projectKeys.lists(),
      });
    },
  });
}

/* -------------------------------------------------------------------------- */
/* Update Project                                                             */
/* -------------------------------------------------------------------------- */

export function useUpdateProject() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      request,
    }: {
      projectId: string;
      request: UpdateProjectRequest;
    }) =>
      updateProject(
        projectId,
        request,
      ),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: projectKeys.detail(
            variables.projectId,
          ),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.lists(),
        }),
      ]);
    },
  });
}

/* -------------------------------------------------------------------------- */
/* Archive Project                                                            */
/* -------------------------------------------------------------------------- */

export function useArchiveProject() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (
      projectId: string,
    ) =>
      archiveProject(projectId),

    onSuccess: async (_, projectId) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: projectKeys.detail(
            projectId,
          ),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.lists(),
        }),
      ]);
    },
  });
}

/* -------------------------------------------------------------------------- */
/* Restore Project                                                            */
/* -------------------------------------------------------------------------- */

export function useRestoreProject() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (
      projectId: string,
    ) =>
      restoreProject(projectId),

    onSuccess: async (_, projectId) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: projectKeys.detail(
            projectId,
          ),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.lists(),
        }),
      ]);
    },
  });
}

/* -------------------------------------------------------------------------- */
/* Add Project Member                                                         */
/* -------------------------------------------------------------------------- */

export function useAddProjectMember() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      request,
    }: {
      projectId: string;
      request: AddProjectMemberRequest;
    }) =>
      addProjectMember(
        projectId,
        request,
      ),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: projectKeys.detail(
            variables.projectId,
          ),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.lists(),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.members(
            variables.projectId,
          ),
        }),
      ]);
    },
  });
}

/* -------------------------------------------------------------------------- */
/* Update Member Role                                                         */
/* -------------------------------------------------------------------------- */

export function useUpdateProjectMemberRole() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      userId,
      request,
    }: {
      projectId: string;
      userId: string;
      request: UpdateProjectMemberRoleRequest;
    }) =>
      updateProjectMemberRole(
        projectId,
        userId,
        request,
      ),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: projectKeys.detail(
            variables.projectId,
          ),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.members(
            variables.projectId,
          ),
        }),
      ]);
    },
  });
}

/* -------------------------------------------------------------------------- */
/* Remove Project Member                                                      */
/* -------------------------------------------------------------------------- */

export function useRemoveProjectMember() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      userId,
    }: {
      projectId: string;
      userId: string;
    }) =>
      removeProjectMember(
        projectId,
        userId,
      ),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: projectKeys.detail(
            variables.projectId,
          ),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.members(
            variables.projectId,
          ),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.lists(),
        }),
      ]);
    },
  });
}

/* -------------------------------------------------------------------------- */
/* Invite Project Member                                                      */
/* -------------------------------------------------------------------------- */

export function useInviteProjectMember() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      request,
    }: {
      projectId: string;
      request: InviteProjectMemberRequest;
    }) =>
      inviteProjectMember(
        projectId,
        request,
      ),

    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries({
        queryKey: projectKeys.invitations(
          variables.projectId,
        ),
      });
    },
  });
}

/* -------------------------------------------------------------------------- */
/* Revoke Project Invitation                                                  */
/* -------------------------------------------------------------------------- */

export function useRevokeProjectInvitation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      invitationId,
    }: {
      projectId: string;
      invitationId: string;
    }) =>
      revokeProjectInvitation(
        projectId,
        invitationId,
      ),

    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries({
        queryKey: projectKeys.invitations(
          variables.projectId,
        ),
      });
    },
  });
}

export function useAcceptProjectInvitation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (token: string) =>
      acceptProjectInvitation(token),

    onSuccess: async (result) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: projectKeys.detail(result.projectId),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.members(result.projectId),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.invitations(result.projectId),
        }),

        queryClient.invalidateQueries({
          queryKey: projectKeys.lists(),
        }),
      ]);
    },
  });
}

export function useDeclineProjectInvitation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (token: string) =>
      declineProjectInvitation(token),

    onSuccess: async (result) => {
      await queryClient.invalidateQueries({
        queryKey: projectKeys.invitations(result.projectId),
      });
    },
  });
}

export function useMoveWorkItemToSprint() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      projectId,
      workItemId,
      sprintId,
    }: {
      projectId: string;
      workItemId: string;
      sprintId: string;
    }) => moveWorkItemToSprint(workItemId, sprintId),

    onSuccess: (_, variables) =>
      invalidateProjectWorkItems(
        queryClient,
        variables.projectId,
      ),
  });
}