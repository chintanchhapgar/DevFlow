import { projectApiClient } from "@/lib/api/project-api-client";

export type Worklog = {
  worklogId: string;
  workItemId: string;
  userId: string;
  description: string | null;
  startedAtUtc: string;
  endedAtUtc: string | null;
  minutesSpent: number;
  isRunning: boolean;
};

export type CreateWorklogRequest = {
  workItemId: string;
  description?: string | null;
  startedAtUtc: string;
  endedAtUtc: string;
};

export async function getWorklogs(
  workItemId: string,
): Promise<Worklog[]> {
  const response = await projectApiClient.get(
    `/api/work-items/${workItemId}/worklogs`,
  );

  return response.data.data;
}

export async function createWorklog(
  request: CreateWorklogRequest,
): Promise<Worklog> {
  const response = await projectApiClient.post(
    "/api/worklogs",
    request,
  );

  return response.data.data;
}

export async function startTimer(
  workItemId: string,
  description?: string | null,
): Promise<Worklog> {
  const response = await projectApiClient.post(
    "/api/worklogs/start",
    {
      workItemId,
      description,
    },
  );

  return response.data.data;
}

export async function stopTimer(
  workItemId: string,
): Promise<Worklog> {
  const response = await projectApiClient.post(
    "/api/worklogs/stop",
    {
      workItemId,
    },
  );

  return response.data.data;
}