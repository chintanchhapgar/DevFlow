import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";

import {
  createWorklog,
  getWorklogs,
  startTimer,
  stopTimer,
  type CreateWorklogRequest,
} from "../api/worklogs-api";

export const worklogKeys = {
  all: ["worklogs"] as const,

  list: (workItemId: string) =>
    [...worklogKeys.all, workItemId] as const,
};

export function useWorklogs(workItemId: string | undefined) {
  return useQuery({
    queryKey: workItemId
      ? worklogKeys.list(workItemId)
      : worklogKeys.all,

    queryFn: () => {
      if (!workItemId) {
        throw new Error("Work item ID is required.");
      }

      return getWorklogs(workItemId);
    },

    enabled: Boolean(workItemId),
    staleTime: 15_000,
    refetchOnWindowFocus: false,
  });
}

function invalidateWorklogs(
  queryClient: ReturnType<typeof useQueryClient>,
  workItemId: string,
) {
  return queryClient.invalidateQueries({
    queryKey: worklogKeys.list(workItemId),
  });
}

export function useStartTimer() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      workItemId,
      description,
    }: {
      workItemId: string;
      description?: string | null;
    }) => startTimer(workItemId, description),

    onSuccess: (_, variables) =>
      invalidateWorklogs(queryClient, variables.workItemId),
  });
}

export function useStopTimer() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      workItemId,
    }: {
      workItemId: string;
    }) => stopTimer(workItemId),

    onSuccess: (_, variables) =>
      invalidateWorklogs(queryClient, variables.workItemId),
  });
}

export function useCreateWorklog() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: CreateWorklogRequest) =>
      createWorklog(request),

    onSuccess: (_, variables) =>
      invalidateWorklogs(queryClient, variables.workItemId),
  });
}