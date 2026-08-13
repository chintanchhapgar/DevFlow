import { useMemo } from "react";
import { useQueries } from "@tanstack/react-query";

import { getWorklogs, type Worklog } from "../api/worklogs-api";
import { useMyWork } from "./use-my-work";

export type MyTimeEntry = Worklog & {
  workItemTitle: string;
  workItemKey: string;
  projectId: string;
  projectName: string;
};

export function useMyTime() {
  const myWorkQuery = useMyWork();

  const worklogQueries = useQueries({
    queries: myWorkQuery.items.map((workItem) => ({
      queryKey: ["my-time", workItem.id] as const,

      queryFn: async () => {
        const worklogs = await getWorklogs(workItem.id);

        return worklogs.map(
          (worklog): MyTimeEntry => ({
            ...worklog,
            workItemTitle: workItem.title,
            workItemKey: workItem.key,
            projectId: workItem.projectId,
            projectName: workItem.projectName,
          }),
        );
      },

      enabled: myWorkQuery.items.length > 0,
      staleTime: 15_000,
      refetchOnWindowFocus: false,
    })),
  });

  const entries = useMemo(
    () =>
      worklogQueries
        .flatMap((query) => query.data ?? [])
        .sort(
          (left, right) =>
            new Date(right.startedAtUtc).getTime() -
            new Date(left.startedAtUtc).getTime(),
        ),
    [worklogQueries],
  );

  return {
    entries,
    isLoading:
      myWorkQuery.isLoading ||
      worklogQueries.some((query) => query.isLoading),

    isError:
      myWorkQuery.isError ||
      worklogQueries.some((query) => query.isError),

    refetch: async () => {
      await Promise.all([
        myWorkQuery.refetch(),
        ...worklogQueries.map((query) => query.refetch()),
      ]);
    },
  };
}