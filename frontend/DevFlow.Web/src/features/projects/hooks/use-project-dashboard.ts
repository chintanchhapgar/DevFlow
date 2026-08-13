import { useQuery } from "@tanstack/react-query";

import { getProjectDashboard } from "../api/project-dashboard-api";

export const projectDashboardKeys = {
  all: ["project-dashboard"] as const,

  detail: (projectId: string) =>
    [...projectDashboardKeys.all, projectId] as const,
};

export function useProjectDashboard(
  projectId: string | undefined,
) {
  return useQuery({
    queryKey: projectId
      ? projectDashboardKeys.detail(projectId)
      : projectDashboardKeys.all,

    queryFn: () => {
      if (!projectId) {
        throw new Error("Project ID is required.");
      }

      return getProjectDashboard(projectId);
    },

    enabled: Boolean(projectId),
    staleTime: 30_000,
    refetchOnWindowFocus: false,
  });
}