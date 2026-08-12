export interface DashboardStat {
  label: string;
  value: string;
  description: string;
  trend?: string;
}

export interface RecentProject {
  id: string;
  name: string;
  description: string;
  status: "Active" | "Planning" | "Completed";
  progress: number;
}

export interface RecentActivityItem {
  id: string;
  title: string;
  description: string;
  time: string;
  type: "project" | "task" | "member";
}