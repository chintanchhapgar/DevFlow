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

export type SaveSprintRequest = {
  name: string;
  goal: string | null;
  startDate: string;
  endDate: string;
};

export type CreateSprintResponse = {
  sprintId: string;
  projectId: string;
  name: string;
};

export type SprintLifecycleResponse = {
  sprintId: string;
  status: SprintStatus;
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

export async function createSprint(projectId: string, request: SaveSprintRequest): Promise<CreateSprintResponse> {
  const response = await projectApiClient.post(`/api/projects/${projectId}/sprints`, request);
  return response.data.data;
}

export async function updateSprint(sprintId: string, request: SaveSprintRequest): Promise<{ sprintId: string; name: string }> {
  const response = await projectApiClient.put(`/api/sprints/${sprintId}`, request);
  return response.data.data;
}

export async function deleteSprint(sprintId: string): Promise<void> {
  await projectApiClient.delete(`/api/sprints/${sprintId}`);
}

export async function startSprint(sprintId: string): Promise<SprintLifecycleResponse> {
  const response = await projectApiClient.post(`/api/sprints/${sprintId}/start`);
  return response.data.data;
}

export async function completeSprint(sprintId: string): Promise<SprintLifecycleResponse> {
  const response = await projectApiClient.post(`/api/sprints/${sprintId}/complete`);
  return response.data.data;
}
