import { useProfile } from "@/features/auth/hooks/use-profile";

import { DashboardStats } from "../components/DashboardStats";
import { RecentActivity } from "../components/RecentActivity";
import { RecentProjects } from "../components/RecentProjects";

import type {
  DashboardStat,
  RecentActivityItem,
  RecentProject,
} from "../types/dashboard";

const stats: DashboardStat[] = [
  {
    label: "Projects",
    value: "12",
    description: "Projects in your workspace",
    trend: "+2 this month",
  },
  {
    label: "Tasks",
    value: "48",
    description: "Tasks currently assigned",
    trend: "+8 this week",
  },
  {
    label: "Completed",
    value: "31",
    description: "Tasks completed",
    trend: "+12%",
  },
  {
    label: "Team Members",
    value: "8",
    description: "Members in your workspace",
  },
];

const projects: RecentProject[] = [
  {
    id: "1",
    name: "DevFlow",
    description: "Microservices project management platform",
    status: "Active",
    progress: 72,
  },
  {
    id: "2",
    name: "Client Portal",
    description: "Customer project management portal",
    status: "Active",
    progress: 58,
  },
  {
    id: "3",
    name: "Mobile Application",
    description: "Cross-platform mobile application",
    status: "Planning",
    progress: 24,
  },
];

const activities: RecentActivityItem[] = [
  {
    id: "1",
    title: "Task completed",
    description: "Authentication session management completed",
    time: "Today",
    type: "task",
  },
  {
    id: "2",
    title: "Project updated",
    description: "DevFlow project progress was updated",
    time: "Yesterday",
    type: "project",
  },
  {
    id: "3",
    title: "New team member",
    description: "A new member joined the workspace",
    time: "2 days ago",
    type: "member",
  },
];

export function DashboardPage() {
  const {
    data: profile,
    isLoading,
  } = useProfile();

  if (isLoading) {
    return (
      <div className="flex min-h-[50vh] items-center justify-center">
        <div className="flex items-center gap-3 text-sm text-[var(--devflow-text-muted)]">
          <div
            className="
              h-5
              w-5
              animate-spin
              rounded-full
              border-2
              border-[var(--devflow-border)]
              border-t-[var(--devflow-primary)]
            "
          />

          Loading dashboard...
        </div>
      </div>
    );
  }

  return (
    <div className="mx-auto w-full max-w-7xl space-y-8">
      {/* Header */}
      <div>
        <p className="text-sm font-medium text-[var(--devflow-primary)]">
          Workspace
        </p>

        <h1 className="mt-1 text-2xl font-semibold tracking-tight text-[var(--devflow-text)]">
          Welcome back
          {profile?.firstName
            ? `, ${profile.firstName}`
            : ""}
        </h1>

        <p className="mt-1.5 text-sm text-[var(--devflow-text-muted)]">
          Here&apos;s what&apos;s happening across your DevFlow
          workspace.
        </p>
      </div>

      {/* Statistics */}
      <DashboardStats stats={stats} />

      {/* Main Content */}
      <div className="grid gap-6 xl:grid-cols-[1.5fr_1fr]">
        <RecentProjects projects={projects} />

        <RecentActivity activities={activities} />
      </div>
    </div>
  );
}