import { projectApiClient } from "@/lib/api/project-api-client";

export interface ProjectDashboard {
  project: {
    projectId: string;
    key: string;
    name: string;
    description: string | null;
    memberCount: number;
  };

  metrics: {
    totalWorkItems: number;
    todo: number;
    inProgress: number;
    review: number;
    done: number;
  };

  activeSprint: {
    sprintId: string;
    name: string;
    startDate: string;
    endDate: string;
    remainingDays: number;
    completionPercentage: number;
  } | null;

  assignedToMe: {
    workItemId: string;
    key: string;
    title: string;
    status: string | number;
    priority: string | number;
    dueDate: string | null;
  }[];

  recentActivities: {
    id: string;
    type: string;
    message: string;
    createdOnUtc: string;
  }[];
}

export async function getProjectDashboard(
  projectId: string,
): Promise<ProjectDashboard> {
  const response = await projectApiClient.get(
    `/api/projects/${projectId}/dashboard`,
  );

  return response.data.data;
}