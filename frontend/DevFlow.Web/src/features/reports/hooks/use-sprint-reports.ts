import { useQueries } from "@tanstack/react-query";

import { getSprints, type Sprint } from "@/features/projects/api/sprints-api";
import { useProjects } from "@/features/projects/hooks/use-projects";

export type ProjectSprint = Sprint & {
  projectId: string;
  projectName: string;
  projectKey: string;
};

export function useProjectSprints() {
  const projectsQuery = useProjects({
    page: 1,
    pageSize: 100,
  });

  const projects = projectsQuery.data?.items ?? [];

  const sprintQueries = useQueries({
    queries: projects.map((project) => ({
      queryKey: ["project-sprints", project.projectId] as const,

      queryFn: async () => {
        const result = await getSprints(project.projectId);

        return result.items.map(
          (sprint): ProjectSprint => ({
            ...sprint,
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

  return {
    sprints: sprintQueries
      .flatMap((query) => query.data ?? [])
      .sort(
        (left, right) =>
          new Date(right.startDate).getTime() -
          new Date(left.startDate).getTime(),
      ),

    isLoading:
      projectsQuery.isLoading ||
      sprintQueries.some((query) => query.isLoading),

    isError:
      projectsQuery.isError ||
      sprintQueries.some((query) => query.isError),
  };
}