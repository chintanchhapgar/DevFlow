import { projectApiClient } from "@/lib/api/project-api-client";

export type SprintStatus =
  | "Planned"
  | "Active"
  | "Completed"
  | "Cancelled"
  | number;

export type Sprint = {
  sprintId: string;
  name: string;
  goal: string | null;
  status: SprintStatus;
  startDate: string;
  endDate: string;
};

type PagedSprints = {
  items: Sprint[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
};

export async function getSprints(
  projectId: string,
): Promise<PagedSprints> {
  const response = await projectApiClient.get(
    `/api/projects/${projectId}/sprints`,
    {
      params: {
        page: 1,
        pageSize: 100,
      },
    },
  );

  return response.data.data;
}