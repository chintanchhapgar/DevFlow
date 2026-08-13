import { useMemo } from "react";
import { useQueries } from "@tanstack/react-query";

import {
  getWorkItems,
  type WorkItem,
} from "../api/project-resources-api";
import { useProjects } from "./use-projects";

export type MyWorkItem = WorkItem & {
  projectId: string;
  projectName: string;
  projectKey: string;
};

export function useMyWork() {
  const projectsQuery = useProjects({
    page: 1,
    pageSize: 100,
  });

  const projects = projectsQuery.data?.items ?? [];

  const workItemQueries = useQueries({
    queries: projects.map((project) => ({
      queryKey: ["my-work", project.projectId] as const,

      queryFn: async () => {
        const result = await getWorkItems(
          project.projectId,
          {
            page: 1,
            pageSize: 100,
          },
        );

        return result.items.map(
          (workItem): MyWorkItem => ({
            ...workItem,
            projectId: project.projectId,
            projectName: project.name,
            projectKey: project.key,
          }),
        );
      },

      enabled: projectsQuery.isSuccess,
      staleTime: 30_000,
      refetchOnWindowFocus: false,
    })),
  });

  const items = useMemo(
    () =>
      workItemQueries
        .flatMap((query) => query.data ?? [])
        .sort((left, right) => {
          if (!left.dueDate) {
            return 1;
          }

          if (!right.dueDate) {
            return -1;
          }

          return (
            new Date(left.dueDate).getTime() -
            new Date(right.dueDate).getTime()
          );
        }),
    [workItemQueries],
  );

  const isLoading =
    projectsQuery.isLoading ||
    workItemQueries.some((query) => query.isLoading);

  const isError =
    projectsQuery.isError ||
    workItemQueries.some((query) => query.isError);

  async function refetch() {
    await Promise.all([
      projectsQuery.refetch(),
      ...workItemQueries.map((query) => query.refetch()),
    ]);
  }

  return {
    items,
    isLoading,
    isError,
    refetch,
  };
}