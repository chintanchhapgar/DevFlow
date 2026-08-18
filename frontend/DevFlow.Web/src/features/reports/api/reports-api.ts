import { projectApiClient } from "@/lib/api/project-api-client";

export interface ProjectSummaryReport {
  projectId: string;
  projectKey: string;
  projectName: string;
  totalWorkItems: number;
  todoCount: number;
  inProgressCount: number;
  reviewCount: number;
  doneCount: number;
  totalSprints: number;
  activeSprints: number;
  completedSprints: number;
  totalMembers: number;
}

export interface VelocitySprint {
  sprintId: string;
  sprintName: string;
  committed: number;
  completed: number;
}

export interface WorkloadMember {
  userId: string;
  totalWorkItems: number;
  totalEstimateHours: number;
  workItems: { id: string; key: string; title: string; estimateHours: number | null }[];
}

export interface BurndownReport {
  sprintId: string;
  sprintName: string;
  points: { date: string; remaining: number; completed: number; ideal: number }[];
}

export async function getProjectSummary(projectId: string): Promise<ProjectSummaryReport> {
  const response = await projectApiClient.get(`/api/projects/${projectId}/reports/summary`);
  return response.data.data;
}

export async function getProjectVelocity(projectId: string): Promise<VelocitySprint[]> {
  const response = await projectApiClient.get(`/api/projects/${projectId}/reports/velocity`);
  return response.data.data.sprints;
}

export async function getProjectWorkload(projectId: string): Promise<WorkloadMember[]> {
  const response = await projectApiClient.get(`/api/projects/${projectId}/reports/workload`);
  return response.data.data.members;
}

export async function getSprintBurndown(sprintId: string): Promise<BurndownReport> {
  const response = await projectApiClient.get(`/api/sprints/${sprintId}/reports/burndown`);
  return response.data.data;
}
