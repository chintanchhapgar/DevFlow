import { useQuery } from "@tanstack/react-query";

import {
  getProjectSummary,
  getProjectVelocity,
  getProjectWorkload,
  getSprintBurndown,
} from "../api/reports-api";

export function useProjectReportSummary(projectId: string | null) {
  return useQuery({ queryKey: ["reports", "summary", projectId], queryFn: () => getProjectSummary(projectId!), enabled: Boolean(projectId), staleTime: 30_000 });
}

export function useProjectVelocity(projectId: string | null) {
  return useQuery({ queryKey: ["reports", "velocity", projectId], queryFn: () => getProjectVelocity(projectId!), enabled: Boolean(projectId), staleTime: 30_000 });
}

export function useProjectWorkload(projectId: string | null) {
  return useQuery({ queryKey: ["reports", "workload", projectId], queryFn: () => getProjectWorkload(projectId!), enabled: Boolean(projectId), staleTime: 30_000 });
}

export function useSprintBurndown(sprintId: string | null) {
  return useQuery({ queryKey: ["reports", "burndown", sprintId], queryFn: () => getSprintBurndown(sprintId!), enabled: Boolean(sprintId), staleTime: 30_000 });
}
