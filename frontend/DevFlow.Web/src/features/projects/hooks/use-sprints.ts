import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import {
  getSprints,
  createSprint,
  updateSprint,
  deleteSprint,
  startSprint,
  completeSprint,
  type SaveSprintRequest,
  type Sprint,
} from "../api/sprints-api";
import { projectResourceKeys } from "./use-project-resources";

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

function useSprintMutation<TVariables extends { projectId: string }>(
  mutationFn: (variables: TVariables) => Promise<unknown>,
) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn,
    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries({ queryKey: ["project-sprints", variables.projectId] });
      await queryClient.invalidateQueries({ queryKey: projectResourceKeys.workItems(variables.projectId) });
    },
  });
}

export function useCreateSprint() {
  return useSprintMutation((variables: { projectId: string; request: SaveSprintRequest }) => createSprint(variables.projectId, variables.request));
}

export function useUpdateSprint() {
  return useSprintMutation((variables: { projectId: string; sprintId: string; request: SaveSprintRequest }) => updateSprint(variables.sprintId, variables.request));
}

export function useDeleteSprint() {
  return useSprintMutation((variables: { projectId: string; sprintId: string }) => deleteSprint(variables.sprintId));
}

export function useStartSprint() {
  return useSprintMutation((variables: { projectId: string; sprintId: string }) => startSprint(variables.sprintId));
}

export function useCompleteSprint() {
  return useSprintMutation((variables: { projectId: string; sprintId: string }) => completeSprint(variables.sprintId));
}
