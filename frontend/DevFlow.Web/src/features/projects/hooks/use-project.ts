import { useQuery } from "@tanstack/react-query";

import { getProject } from "../api/projects-api";

export const projectKeys = {
  all: ["projects"] as const,

  lists: () =>
    [...projectKeys.all, "list"] as const,

  list: (
    params: unknown,
  ) =>
    [
      ...projectKeys.lists(),
      params,
    ] as const,

  details: () =>
    [...projectKeys.all, "detail"] as const,

  detail: (
    projectId: string,
  ) =>
    [
      ...projectKeys.details(),
      projectId,
    ] as const,

  members: (
    projectId: string,
  ) =>
    [
      ...projectKeys.detail(projectId),
      "members",
    ] as const,

  invitations: (
    projectId: string,
  ) =>
    [
      ...projectKeys.detail(projectId),
      "invitations",
    ] as const,
};

export function useProject(
  projectId: string | undefined,
) {
  return useQuery({
    queryKey: projectId
      ? projectKeys.detail(projectId)
      : projectKeys.details(),

    queryFn: () => {
      if (!projectId) {
        throw new Error(
          "Project ID is required.",
        );
      }

      return getProject(projectId);
    },

    enabled: Boolean(projectId),

    staleTime: 30_000,

    refetchOnWindowFocus: false,
  });
}