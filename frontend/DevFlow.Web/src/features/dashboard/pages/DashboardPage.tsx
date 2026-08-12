import {
  Activity,
  ArrowRight,
  CheckCircle2,
  CircleAlert,
  Clock3,
  FolderKanban,
  ListTodo,
  Plus,
  Users,
} from "lucide-react";

import { useProfile } from "@/features/auth/hooks/use-profile";

export function DashboardPage() {
  const {
    data: profile,
    isLoading,
    isError,
  } = useProfile();

  if (isLoading) {
    return (
      <div className="flex min-h-[calc(100vh-4rem)] items-center justify-center bg-slate-50">
        <div className="flex items-center gap-3 text-sm text-slate-500">
          <div className="h-5 w-5 animate-spin rounded-full border-2 border-slate-200 border-t-blue-600" />
          Loading dashboard...
        </div>
      </div>
    );
  }

  if (isError || !profile) {
    return (
      <div className="flex min-h-[calc(100vh-4rem)] items-center justify-center bg-slate-50">
        <div className="rounded-xl border border-red-200 bg-white px-6 py-5 text-center shadow-sm">
          <CircleAlert className="mx-auto h-6 w-6 text-red-500" />

          <p className="mt-3 text-sm font-medium text-slate-900">
            Unable to load your profile.
          </p>

          <p className="mt-1 text-xs text-slate-500">
            Please try refreshing the page.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-full bg-slate-50 text-slate-900">

      {/* Page Header */}
      <section className="border-b border-slate-200 bg-white">
        <div className="mx-auto max-w-7xl px-6 py-7">

          <div className="flex flex-col gap-5 sm:flex-row sm:items-center sm:justify-between">

            <div>
              <p className="text-sm font-medium text-blue-600">
                Dashboard
              </p>

              <h1 className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
                Welcome back, {profile.firstName}
              </h1>

              <p className="mt-1 text-sm text-slate-500">
                Here's what's happening across your workspace.
              </p>
            </div>

            <button
              type="button"
              className="
                inline-flex
                items-center
                justify-center
                gap-2
                rounded-lg
                bg-blue-600
                px-4
                py-2.5
                text-sm
                font-medium
                text-white
                shadow-sm
                transition
                hover:bg-blue-700
                focus:outline-none
                focus:ring-2
                focus:ring-blue-500/30
              "
            >
              <Plus className="h-4 w-4" />

              New Project
            </button>

          </div>
        </div>
      </section>

      {/* Dashboard Content */}
      <main className="mx-auto max-w-7xl space-y-6 px-6 py-6">

        {/* Statistics */}
        <section className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">

          <StatCard
            title="Projects"
            value="4"
            description="+2 this month"
            icon={FolderKanban}
          />

          <StatCard
            title="Open Tasks"
            value="18"
            description="6 due this week"
            icon={ListTodo}
          />

          <StatCard
            title="Completed"
            value="42"
            description="+12 this month"
            icon={CheckCircle2}
          />

          <StatCard
            title="Team Members"
            value="8"
            description="2 active now"
            icon={Users}
          />

        </section>

        {/* Main Grid */}
        <section className="grid gap-6 xl:grid-cols-[minmax(0,1.7fr)_minmax(320px,1fr)]">

          {/* Recent Activity */}
          <DashboardCard
            title="Recent Activity"
            description="Latest activity across your workspace"
            action="View all"
          >
            <div className="divide-y divide-slate-100">

              <ActivityItem
                icon={FolderKanban}
                title="Project Alpha was updated"
                description="Project settings were updated"
                time="10 minutes ago"
              />

              <ActivityItem
                icon={CheckCircle2}
                title="Task completed"
                description="Implement authentication flow"
                time="32 minutes ago"
              />

              <ActivityItem
                icon={ListTodo}
                title="New task created"
                description="Add session management"
                time="1 hour ago"
              />

              <ActivityItem
                icon={Users}
                title="New team member joined"
                description="Alex joined Project Alpha"
                time="2 hours ago"
              />

              <ActivityItem
                icon={Activity}
                title="Project activity recorded"
                description="Deployment pipeline updated"
                time="3 hours ago"
              />

            </div>
          </DashboardCard>

          {/* Upcoming Tasks */}
          <DashboardCard
            title="Upcoming Tasks"
            description="Tasks that need your attention"
            action="View all"
          >
            <div className="space-y-3">

              <TaskItem
                title="Complete session management"
                project="DevFlow"
                due="Today"
                priority="High"
              />

              <TaskItem
                title="Implement notification service"
                project="DevFlow"
                due="Tomorrow"
                priority="Medium"
              />

              <TaskItem
                title="Review API documentation"
                project="Identity Service"
                due="Aug 14"
                priority="Low"
              />

              <TaskItem
                title="Create project dashboard"
                project="DevFlow"
                due="Aug 16"
                priority="Medium"
              />

            </div>
          </DashboardCard>

        </section>

        {/* Workspace Status */}
        <section className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm">

          <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">

            <div className="flex items-center gap-3">

              <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-emerald-50">
                <CheckCircle2 className="h-5 w-5 text-emerald-600" />
              </div>

              <div>
                <h3 className="text-sm font-semibold text-slate-900">
                  Workspace is running smoothly
                </h3>

                <p className="mt-0.5 text-xs text-slate-500">
                  All systems are operational.
                </p>
              </div>

            </div>

            <div className="flex items-center gap-2 text-xs font-medium text-emerald-600">
              <span className="h-2 w-2 rounded-full bg-emerald-500" />

              All systems operational

              <ArrowRight className="ml-1 h-3.5 w-3.5" />
            </div>

          </div>

        </section>

      </main>
    </div>
  );
}

/* -------------------------------------------------------------------------- */
/* Components                                                                 */
/* -------------------------------------------------------------------------- */

function StatCard({
  title,
  value,
  description,
  icon: Icon,
}: {
  title: string;
  value: string;
  description: string;
  icon: React.ComponentType<{
    className?: string;
  }>;
}) {
  return (
    <div className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm transition hover:shadow-md">

      <div className="flex items-start justify-between">

        <div>
          <p className="text-sm font-medium text-slate-500">
            {title}
          </p>

          <p className="mt-2 text-2xl font-semibold tracking-tight text-slate-900">
            {value}
          </p>

          <p className="mt-1 text-xs text-slate-500">
            {description}
          </p>
        </div>

        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-blue-50">
          <Icon className="h-5 w-5 text-blue-600" />
        </div>

      </div>
    </div>
  );
}

function DashboardCard({
  title,
  description,
  action,
  children,
}: {
  title: string;
  description: string;
  action: string;
  children: React.ReactNode;
}) {
  return (
    <div className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">

      <div className="flex items-center justify-between border-b border-slate-100 px-5 py-4">

        <div>
          <h2 className="text-sm font-semibold text-slate-900">
            {title}
          </h2>

          <p className="mt-0.5 text-xs text-slate-500">
            {description}
          </p>
        </div>

        <button
          type="button"
          className="inline-flex items-center gap-1 text-xs font-medium text-blue-600 transition hover:text-blue-700"
        >
          {action}

          <ArrowRight className="h-3.5 w-3.5" />
        </button>

      </div>

      <div className="p-2">
        {children}
      </div>

    </div>
  );
}

function ActivityItem({
  icon: Icon,
  title,
  description,
  time,
}: {
  icon: React.ComponentType<{
    className?: string;
  }>;
  title: string;
  description: string;
  time: string;
}) {
  return (
    <div className="flex items-center gap-3 rounded-lg px-3 py-3 transition hover:bg-slate-50">

      <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-slate-100">
        <Icon className="h-4 w-4 text-slate-600" />
      </div>

      <div className="min-w-0 flex-1">

        <p className="truncate text-sm font-medium text-slate-900">
          {title}
        </p>

        <p className="truncate text-xs text-slate-500">
          {description}
        </p>

      </div>

      <span className="shrink-0 text-xs text-slate-400">
        {time}
      </span>

    </div>
  );
}

function TaskItem({
  title,
  project,
  due,
  priority,
}: {
  title: string;
  project: string;
  due: string;
  priority: "High" | "Medium" | "Low";
}) {
  const priorityClasses = {
    High: "bg-red-50 text-red-600",
    Medium: "bg-amber-50 text-amber-600",
    Low: "bg-slate-100 text-slate-600",
  };

  return (
    <div className="rounded-lg border border-slate-100 p-3 transition hover:border-slate-200 hover:bg-slate-50">

      <div className="flex items-start gap-3">

        <div className="mt-0.5 h-4 w-4 rounded-full border-2 border-slate-300" />

        <div className="min-w-0 flex-1">

          <p className="text-sm font-medium text-slate-900">
            {title}
          </p>

          <div className="mt-2 flex flex-wrap items-center gap-2">

            <span className="text-xs text-slate-500">
              {project}
            </span>

            <span className="text-slate-300">
              •
            </span>

            <span className="inline-flex items-center gap-1 text-xs text-slate-500">
              <Clock3 className="h-3 w-3" />
              {due}
            </span>

            <span
              className={[
                "rounded-full px-2 py-0.5 text-[10px] font-medium",
                priorityClasses[priority],
              ].join(" ")}
            >
              {priority}
            </span>

          </div>

        </div>

      </div>

    </div>
  );
}