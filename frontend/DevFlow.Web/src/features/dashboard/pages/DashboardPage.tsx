import { useMemo } from "react";
import {
  ArrowRight,
  CheckCircle2,
  CircleDot,
  ClipboardList,
  Clock3,
  FolderKanban,
  ListTodo,
} from "lucide-react";
import { Link } from "react-router-dom";

import { useProfile } from "@/features/auth/hooks/use-profile";
import {
  WorkItemStatus,
} from "@/features/projects/api/project-resources-api";
import { useMyWork } from "@/features/projects/hooks/use-my-work";
import { useProjects } from "@/features/projects/hooks/use-projects";

function statusValue(value: string | number) {
  if (typeof value === "number") {
    return value;
  }

  const statuses: Record<string, WorkItemStatus> = {
    todo: WorkItemStatus.Todo,
    "to do": WorkItemStatus.Todo,
    inprogress: WorkItemStatus.InProgress,
    "in progress": WorkItemStatus.InProgress,
    inreview: WorkItemStatus.InReview,
    "in review": WorkItemStatus.InReview,
    testing: WorkItemStatus.Testing,
    done: WorkItemStatus.Done,
    cancelled: WorkItemStatus.Cancelled,
  };

  return statuses[value.toLowerCase()] ?? WorkItemStatus.Todo;
}

function formatDate(value?: string | null) {
  if (!value) {
    return "No due date";
  }

  return new Intl.DateTimeFormat(undefined, {
    day: "numeric",
    month: "short",
  }).format(new Date(value));
}

function formatActivityDate(value?: string | null) {
  if (!value) {
    return "Recently";
  }

  const date = new Date(value);
  const now = new Date();
  const today = new Date(
    now.getFullYear(),
    now.getMonth(),
    now.getDate(),
  );
  const activityDay = new Date(
    date.getFullYear(),
    date.getMonth(),
    date.getDate(),
  );

  const daysAgo = Math.round(
    (today.getTime() - activityDay.getTime()) / 86_400_000,
  );

  if (daysAgo === 0) return "Today";
  if (daysAgo === 1) return "Yesterday";
  if (daysAgo > 1 && daysAgo < 7) return `${daysAgo} days ago`;

  return new Intl.DateTimeFormat(undefined, {
    day: "numeric",
    month: "short",
  }).format(date);
}

export function DashboardPage() {
  const { data: profile, isLoading: isProfileLoading } = useProfile();
  const projectsQuery = useProjects({
    page: 1,
    pageSize: 100,
  });
  const myWorkQuery = useMyWork();

  const dashboard = useMemo(() => {
    const workItems = myWorkQuery.items;
    const now = new Date();

    const completed = workItems.filter(
      (item) => statusValue(item.status) === WorkItemStatus.Done,
    );

    const inProgress = workItems.filter(
      (item) =>
        statusValue(item.status) === WorkItemStatus.InProgress ||
        statusValue(item.status) === WorkItemStatus.InReview ||
        statusValue(item.status) === WorkItemStatus.Testing,
    );

    const overdue = workItems.filter((item) => {
      if (!item.dueDate) {
        return false;
      }

      return (
        new Date(item.dueDate) < now &&
        statusValue(item.status) !== WorkItemStatus.Done &&
        statusValue(item.status) !== WorkItemStatus.Cancelled
      );
    });

    const recentWork = [...workItems]
      .sort((left, right) => {
        const leftDate = new Date(
          left.updatedOnUtc ?? left.createdOnUtc ?? 0,
        ).getTime();
        const rightDate = new Date(
          right.updatedOnUtc ?? right.createdOnUtc ?? 0,
        ).getTime();

        return rightDate - leftDate;
      })
      .slice(0, 6);

    const upcomingWork = [...workItems]
      .filter(
        (item) =>
          item.dueDate &&
          statusValue(item.status) !== WorkItemStatus.Done &&
          statusValue(item.status) !== WorkItemStatus.Cancelled,
      )
      .sort(
        (left, right) =>
          new Date(left.dueDate!).getTime() -
          new Date(right.dueDate!).getTime(),
      )
      .slice(0, 5);

    return {
      completed,
      inProgress,
      overdue,
      recentWork,
      upcomingWork,
    };
  }, [myWorkQuery.items]);

  const isLoading =
    isProfileLoading ||
    projectsQuery.isLoading ||
    myWorkQuery.isLoading;

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div className="h-20 animate-pulse rounded-xl bg-slate-100" />
        <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
          {[0, 1, 2, 3].map((index) => (
            <div
              key={index}
              className="h-32 animate-pulse rounded-2xl bg-slate-100"
            />
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className="mx-auto w-full max-w-7xl space-y-6">
      <div>
        <p className="text-sm font-medium text-[var(--devflow-primary)]">
          Workspace
        </p>

        <h1 className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
          Welcome back
          {profile?.firstName ? `, ${profile.firstName}` : ""}
        </h1>

        <p className="mt-1.5 text-sm text-slate-500">
          Here&apos;s the latest from your projects and assigned work.
        </p>
      </div>

      {projectsQuery.isError || myWorkQuery.isError ? (
        <section className="rounded-2xl border border-amber-200 bg-amber-50 px-5 py-4 text-sm text-amber-800">
          Some dashboard information could not be loaded. Please refresh
          the page to try again.
        </section>
      ) : null}

      <section className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
        <DashboardCard
          label="Projects"
          value={projectsQuery.data?.totalCount ?? 0}
          description="Projects in your workspace"
          icon={FolderKanban}
          iconClassName="bg-sky-50 text-sky-600"
        />

        <DashboardCard
          label="My work"
          value={myWorkQuery.items.length}
          description="Work items across your projects"
          icon={ListTodo}
          iconClassName="bg-violet-50 text-violet-600"
        />

        <DashboardCard
          label="In progress"
          value={dashboard.inProgress.length}
          description="Items currently moving forward"
          icon={CircleDot}
          iconClassName="bg-amber-50 text-amber-600"
        />

        <DashboardCard
          label="Completed"
          value={dashboard.completed.length}
          description="Finished work items"
          icon={CheckCircle2}
          iconClassName="bg-emerald-50 text-emerald-600"
        />
      </section>

      <div className="grid gap-6 xl:grid-cols-[1.35fr_1fr]">
        <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
          <div className="flex items-start justify-between gap-4 border-b border-slate-100 px-5 py-4">
            <div>
              <h2 className="text-sm font-semibold text-slate-900">
                Recent activity
              </h2>
              <p className="mt-1 text-xs text-slate-500">
                Latest work-item updates across your projects.
              </p>
            </div>

            <Link
              to="/activity"
              className="inline-flex shrink-0 items-center gap-1 text-xs font-medium text-[var(--devflow-primary)] hover:underline"
            >
              View all
              <ArrowRight className="h-3.5 w-3.5" />
            </Link>
          </div>

          {dashboard.recentWork.length === 0 ? (
            <EmptyState
              icon={ClipboardList}
              title="No activity yet"
              description="Work-item updates will appear here."
            />
          ) : (
            <div className="divide-y divide-slate-100">
              {dashboard.recentWork.map((item) => {
                const completed =
                  statusValue(item.status) === WorkItemStatus.Done;
                const Icon = completed ? CheckCircle2 : CircleDot;

                return (
                  <Link
                    key={item.id}
                    to={`/projects/${item.projectId}`}
                    className="flex gap-3 px-5 py-4 transition-colors hover:bg-slate-50"
                  >
                    <div
                      className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-xl ${
                        completed
                          ? "bg-emerald-50 text-emerald-600"
                          : "bg-sky-50 text-sky-600"
                      }`}
                    >
                      <Icon className="h-4 w-4" />
                    </div>

                    <div className="min-w-0 flex-1">
                      <div className="flex items-start justify-between gap-3">
                        <p className="truncate text-sm font-medium text-slate-800">
                          {completed ? "Completed" : "Updated"}{" "}
                          <span className="font-normal">{item.title}</span>
                        </p>

                        <span className="shrink-0 text-[11px] text-slate-400">
                          {formatActivityDate(
                            item.updatedOnUtc ?? item.createdOnUtc,
                          )}
                        </span>
                      </div>

                      <p className="mt-1 text-xs text-slate-500">
                        <span className="font-medium text-slate-600">
                          {item.key}
                        </span>
                        {" · "}
                        {item.projectName}
                      </p>
                    </div>
                  </Link>
                );
              })}
            </div>
          )}
        </section>

        <div className="space-y-6">
          <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
            <div className="flex items-start justify-between gap-4 border-b border-slate-100 px-5 py-4">
              <div>
                <h2 className="text-sm font-semibold text-slate-900">
                  Upcoming work
                </h2>
                <p className="mt-1 text-xs text-slate-500">
                  Your next due work items.
                </p>
              </div>

              <Link
                to="/work"
                className="inline-flex shrink-0 items-center gap-1 text-xs font-medium text-[var(--devflow-primary)] hover:underline"
              >
                My work
                <ArrowRight className="h-3.5 w-3.5" />
              </Link>
            </div>

            {dashboard.upcomingWork.length === 0 ? (
              <EmptyState
                icon={Clock3}
                title="Nothing due soon"
                description="Due work items will appear here."
              />
            ) : (
              <div className="divide-y divide-slate-100">
                {dashboard.upcomingWork.map((item) => (
                  <Link
                    key={item.id}
                    to={`/projects/${item.projectId}`}
                    className="flex items-center gap-3 px-5 py-3.5 transition-colors hover:bg-slate-50"
                  >
                    <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-slate-100 text-slate-500">
                      <Clock3 className="h-3.5 w-3.5" />
                    </div>

                    <div className="min-w-0 flex-1">
                      <p className="truncate text-sm font-medium text-slate-800">
                        {item.title}
                      </p>

                      <p className="mt-1 text-xs text-slate-500">
                        {item.projectName}
                      </p>
                    </div>

                    <span className="shrink-0 text-xs font-medium text-slate-500">
                      {formatDate(item.dueDate)}
                    </span>
                  </Link>
                ))}
              </div>
            )}
          </section>

          <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
            <div className="flex items-center gap-3">
              <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-red-50 text-red-600">
                <Clock3 className="h-5 w-5" />
              </div>

              <div>
                <p className="text-sm font-semibold text-slate-900">
                  Needs attention
                </p>

                <p className="mt-0.5 text-xs text-slate-500">
                  Work items past their due date.
                </p>
              </div>
            </div>

            <p className="mt-5 text-3xl font-semibold tracking-tight text-slate-900">
              {dashboard.overdue.length}
            </p>

            <Link
              to="/work"
              className="mt-3 inline-flex items-center gap-1 text-sm font-medium text-[var(--devflow-primary)] hover:underline"
            >
              Review my work
              <ArrowRight className="h-4 w-4" />
            </Link>
          </section>
        </div>
      </div>
    </div>
  );
}

function DashboardCard({
  label,
  value,
  description,
  icon: Icon,
  iconClassName,
}: {
  label: string;
  value: number;
  description: string;
  icon: React.ComponentType<{ className?: string }>;
  iconClassName: string;
}) {
  return (
    <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
      <div className="flex items-start justify-between gap-4">
        <div>
          <p className="text-sm font-medium text-slate-500">{label}</p>
          <p className="mt-2 text-3xl font-semibold tracking-tight text-slate-900">
            {value}
          </p>
        </div>

        <div
          className={`flex h-10 w-10 items-center justify-center rounded-xl ${iconClassName}`}
        >
          <Icon className="h-5 w-5" />
        </div>
      </div>

      <p className="mt-3 text-xs text-slate-400">{description}</p>
    </section>
  );
}

function EmptyState({
  icon: Icon,
  title,
  description,
}: {
  icon: React.ComponentType<{ className?: string }>;
  title: string;
  description: string;
}) {
  return (
    <div className="px-5 py-12 text-center">
      <div className="mx-auto flex h-10 w-10 items-center justify-center rounded-xl bg-slate-50 text-slate-400">
        <Icon className="h-5 w-5" />
      </div>

      <p className="mt-3 text-sm font-medium text-slate-700">{title}</p>
      <p className="mt-1 text-xs text-slate-400">{description}</p>
    </div>
  );
}