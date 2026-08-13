import {
  Activity,
  CheckCircle2,
  CircleDot,
  ClipboardList,
  Clock3,
  Flame,
  Users,
} from "lucide-react";

import { Button } from "@/components/ui/button";

import { useProjectDashboard } from "../hooks/use-project-dashboard";

function formatDate(value: string) {
  return new Intl.DateTimeFormat(undefined, {
    dateStyle: "medium",
  }).format(new Date(value));
}

function formatRelativeDate(value: string) {
  const diffMs = Date.now() - new Date(value).getTime();
  const minutes = Math.max(0, Math.floor(diffMs / 60_000));

  if (minutes < 1) {
    return "Just now";
  }

  if (minutes < 60) {
    return `${minutes}m ago`;
  }

  const hours = Math.floor(minutes / 60);

  if (hours < 24) {
    return `${hours}h ago`;
  }

  const days = Math.floor(hours / 24);

  return `${days}d ago`;
}

function statusLabel(value: string | number) {
  if (typeof value === "number") {
    return (
      {
        1: "To do",
        2: "In progress",
        3: "In review",
        4: "Testing",
        5: "Done",
        6: "Cancelled",
      }[value] ?? "Unknown"
    );
  }

  return value.replace(/([a-z])([A-Z])/g, "$1 $2");
}

function priorityClass(value: string | number) {
  const priority =
    typeof value === "number" ? value : 3;

  if (priority >= 5) {
    return "bg-red-50 text-red-700";
  }

  if (priority === 4) {
    return "bg-orange-50 text-orange-700";
  }

  if (priority === 3) {
    return "bg-blue-50 text-blue-700";
  }

  return "bg-slate-100 text-slate-600";
}

export function ProjectInsightsTab({
  projectId,
}: {
  projectId: string;
}) {
  const dashboardQuery = useProjectDashboard(projectId);

  if (dashboardQuery.isLoading) {
    return (
      <div className="space-y-6">
        <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
          {[0, 1, 2, 3].map((index) => (
            <div
              key={index}
              className="h-32 animate-pulse rounded-2xl bg-slate-100"
            />
          ))}
        </div>

        <div className="grid gap-6 xl:grid-cols-2">
          <div className="h-72 animate-pulse rounded-2xl bg-slate-100" />
          <div className="h-72 animate-pulse rounded-2xl bg-slate-100" />
        </div>
      </div>
    );
  }

  if (dashboardQuery.isError || !dashboardQuery.data) {
    return (
      <section className="rounded-2xl border border-red-200 bg-red-50 p-5">
        <p className="font-medium text-red-800">
          Unable to load project insights.
        </p>

        <p className="mt-1 text-sm text-red-700">
          Please try again.
        </p>

        <Button
          type="button"
          variant="outline"
          size="sm"
          className="mt-4"
          onClick={() => dashboardQuery.refetch()}
        >
          Try again
        </Button>
      </section>
    );
  }

  const { metrics, activeSprint, assignedToMe, recentActivities } =
    dashboardQuery.data;

  const completion =
    metrics.totalWorkItems > 0
      ? Math.round(
          (metrics.done / metrics.totalWorkItems) * 100,
        )
      : 0;

  const metricCards = [
    {
      label: "Total work",
      value: metrics.totalWorkItems,
      hint: "All work items",
      icon: ClipboardList,
      className: "text-slate-600",
    },
    {
      label: "In progress",
      value: metrics.inProgress,
      hint: "Currently active",
      icon: Flame,
      className: "text-blue-600",
    },
    {
      label: "In review",
      value: metrics.review,
      hint: "Awaiting review",
      icon: Clock3,
      className: "text-amber-600",
    },
    {
      label: "Completed",
      value: metrics.done,
      hint: `${completion}% complete`,
      icon: CheckCircle2,
      className: "text-emerald-600",
    },
  ];

  return (
    <div className="space-y-6">
      <section className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        {metricCards.map((metric) => {
          const Icon = metric.icon;

          return (
            <div
              key={metric.label}
              className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm"
            >
              <div className="flex items-start justify-between">
                <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-slate-50">
                  <Icon
                    className={`h-5 w-5 ${metric.className}`}
                  />
                </div>

                <span className="text-xs font-medium text-slate-400">
                  {metric.hint}
                </span>
              </div>

              <p className="mt-5 text-xs font-medium text-slate-500">
                {metric.label}
              </p>

              <p className="mt-1 text-3xl font-semibold tracking-tight text-slate-900">
                {metric.value}
              </p>
            </div>
          );
        })}
      </section>

      <div className="grid gap-6 xl:grid-cols-[minmax(0,1.2fr)_minmax(0,1fr)]">
        <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
          <div className="border-b border-slate-100 px-5 py-4">
            <h2 className="font-semibold text-slate-900">
              Assigned to me
            </h2>

            <p className="mt-1 text-sm text-slate-500">
              Work items currently assigned to you.
            </p>
          </div>

          {assignedToMe.length === 0 ? (
            <div className="flex min-h-56 flex-col items-center justify-center px-5 text-center">
              <CircleDot className="h-7 w-7 text-slate-400" />

              <p className="mt-3 text-sm font-medium text-slate-800">
                Nothing assigned to you
              </p>

              <p className="mt-1 text-sm text-slate-500">
                Assigned work items will appear here.
              </p>
            </div>
          ) : (
            <div className="divide-y divide-slate-100">
              {assignedToMe.map((workItem) => (
                <div
                  key={workItem.workItemId}
                  className="flex items-center gap-3 px-5 py-4"
                >
                  <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-blue-50 text-blue-600">
                    <ClipboardList className="h-4 w-4" />
                  </div>

                  <div className="min-w-0 flex-1">
                    <p className="truncate text-sm font-medium text-slate-900">
                      {workItem.title}
                    </p>

                    <p className="mt-0.5 text-xs text-slate-500">
                      {workItem.key} · {statusLabel(workItem.status)}
                    </p>
                  </div>

                  <div className="text-right">
                    <span
                      className={`rounded-full px-2 py-1 text-[10px] font-semibold ${priorityClass(
                        workItem.priority,
                      )}`}
                    >
                      Priority
                    </span>

                    <p className="mt-1 text-[11px] text-slate-400">
                      {workItem.dueDate
                        ? `Due ${formatDate(workItem.dueDate)}`
                        : "No due date"}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          )}
        </section>

        <div className="space-y-6">
          <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
            <div className="flex items-center gap-3">
              <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-violet-50 text-violet-600">
                <Activity className="h-5 w-5" />
              </div>

              <div>
                <h2 className="font-semibold text-slate-900">
                  Active sprint
                </h2>

                <p className="text-sm text-slate-500">
                  Current sprint progress.
                </p>
              </div>
            </div>

            {activeSprint ? (
              <div className="mt-5">
                <div className="flex items-center justify-between gap-3">
                  <p className="truncate text-sm font-semibold text-slate-800">
                    {activeSprint.name}
                  </p>

                  <span className="text-xs font-medium text-slate-500">
                    {activeSprint.remainingDays} days left
                  </span>
                </div>

                <div className="mt-3 h-2 overflow-hidden rounded-full bg-slate-100">
                  <div
                    className="h-full rounded-full bg-violet-500"
                    style={{
                      width: `${Math.min(
                        Math.max(
                          activeSprint.completionPercentage,
                          0,
                        ),
                        100,
                      )}%`,
                    }}
                  />
                </div>

                <p className="mt-2 text-xs text-slate-500">
                  {Math.round(activeSprint.completionPercentage)}%
                  complete · Ends {formatDate(activeSprint.endDate)}
                </p>
              </div>
            ) : (
              <p className="mt-5 text-sm text-slate-500">
                No active sprint for this project.
              </p>
            )}
          </section>

          <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
            <div className="border-b border-slate-100 px-5 py-4">
              <h2 className="font-semibold text-slate-900">
                Recent activity
              </h2>
            </div>

            {recentActivities.length === 0 ? (
              <div className="px-5 py-8 text-center text-sm text-slate-500">
                No recent activity yet.
              </div>
            ) : (
              <div className="divide-y divide-slate-100">
                {recentActivities.slice(0, 5).map((activity) => (
                  <div
                    key={activity.id}
                    className="flex gap-3 px-5 py-3.5"
                  >
                    <div className="mt-0.5 flex h-7 w-7 shrink-0 items-center justify-center rounded-lg bg-slate-50 text-slate-500">
                      <Activity className="h-3.5 w-3.5" />
                    </div>

                    <div className="min-w-0 flex-1">
                      <p className="text-sm leading-5 text-slate-700">
                        {activity.message}
                      </p>

                      <p className="mt-1 text-xs text-slate-400">
                        {formatRelativeDate(activity.createdOnUtc)}
                      </p>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </section>
        </div>
      </div>

      <section className="flex items-center gap-3 rounded-xl border border-slate-200 bg-slate-50 px-4 py-3 text-sm text-slate-600">
        <Users className="h-4 w-4 text-slate-400" />
        Project team:{" "}
        <span className="font-semibold text-slate-800">
          {dashboardQuery.data.project.memberCount} members
        </span>
      </section>
    </div>
  );
}