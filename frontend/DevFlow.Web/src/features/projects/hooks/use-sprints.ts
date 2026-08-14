import { useQuery } from "@tanstack/react-query";

import {
  getSprints,
  type Sprint,
} from "../api/sprints-api";

export function useProjectSprints(
  projectId: string | undefined,
) {
  return useQuery({
    queryKey: ["project-sprints", projectId],

    queryFn: () => {
      if (!projectId) {
        throw new Error("Project ID is required.");
      }

      return getSprints(projectId);
    },

    enabled: Boolean(projectId),
    staleTime: 30_000,
    refetchOnWindowFocus: false,
  });
}

export type { Sprint };