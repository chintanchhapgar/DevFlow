import { useMemo, useState } from "react";
import {
  AlertTriangle,
  CheckCircle2,
  Download,
  Gauge,
  ListTodo,
  Rocket,
  TrendingDown,
  Search,
} from "lucide-react";

import { Button } from "@/components/ui/button";
import { WorkItemStatus } from "@/features/projects/api/project-resources-api";
import { useMyWork } from "@/features/projects/hooks/use-my-work";
import { Link } from "react-router-dom";
import { Input } from "@/components/ui/input";
import { useProjectSprints } from "../hooks/use-sprint-reports";
import { WorkItemDetailDialog } from "@/features/projects/components/WorkItemDetailDialog";
import { useProject } from "@/features/projects/hooks/use-project";
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

function formatHours(hours: number) {
  return `${Number.isInteger(hours) ? hours : hours.toFixed(1)}h`;
}

function daysBetween(startDate: string, endDate: string) {
  const start = new Date(startDate);
  const end = new Date(endDate);

  return Math.max(
    1,
    Math.ceil(
      (end.getTime() - start.getTime()) / 86_400_000,
    ) + 1,
  );
}

function daysRemaining(endDate: string) {
  const now = new Date();
  const end = new Date(endDate);

  now.setHours(0, 0, 0, 0);
  end.setHours(0, 0, 0, 0);

  return Math.ceil(
    (end.getTime() - now.getTime()) / 86_400_000,
  );
}

function csvCell(value: string | number | null | undefined) {
  return `"${String(value ?? "").replace(/"/g, '""')}"`;
}
type BurndownPoint = {
  date: Date;
  ideal: number;
  actual: number | null;
};

function localDate(value: string) {
  return new Date(`${value.slice(0, 10)}T00:00:00`);
}

function chartDateLabel(date: Date) {
  return new Intl.DateTimeFormat(undefined, {
    day: "numeric",
    month: "short",
  }).format(date);
}

function statusLabel(value: string | number) {
  const labels: Record<number, string> = {
    [WorkItemStatus.Todo]: "To do",
    [WorkItemStatus.InProgress]: "In progress",
    [WorkItemStatus.InReview]: "In review",
    [WorkItemStatus.Testing]: "Testing",
    [WorkItemStatus.Done]: "Done",
    [WorkItemStatus.Cancelled]: "Cancelled",
  };

  return labels[statusValue(value)] ?? "To do";
}

function priorityLabel(value: string | number) {
  const labels: Record<string, string> = {
    lowest: "Lowest",
    low: "Low",
    medium: "Medium",
    high: "High",
    highest: "Highest",
  };

  if (typeof value === "number") {
    return (
      {
        1: "Lowest",
        2: "Low",
        3: "Medium",
        4: "High",
        5: "Highest",
      }[value] ?? "Medium"
    );
  }

  return labels[value.toLowerCase()] ?? "Medium";
}

function priorityClass(value: string | number) {
  const priority = priorityLabel(value).toLowerCase();

  if (priority === "highest") {
    return "bg-red-50 text-red-700";
  }

  if (priority === "high") {
    return "bg-orange-50 text-orange-700";
  }

  if (priority === "medium") {
    return "bg-amber-50 text-amber-700";
  }

  return "bg-slate-100 text-slate-600";
}

export function SprintReportsPage() {
  const sprintsQuery = useProjectSprints();
  const myWorkQuery = useMyWork();

  const [selectedSprintId, setSelectedSprintId] = useState("");
  const [workItemSearch, setWorkItemSearch] = useState("");
    const [workItemStatus, setWorkItemStatus] = useState<
    "all" | WorkItemStatus
    >("all");
const [selectedWorkItemId, setSelectedWorkItemId] =
  useState<string | null>(null);
  const selectedSprint = useMemo(() => {
    if (selectedSprintId) {
      return (
        sprintsQuery.sprints.find(
          (sprint) => sprint.sprintId === selectedSprintId,
        ) ?? null
      );
    }

    return (
      sprintsQuery.sprints.find(
        (sprint) =>
          String(sprint.status).toLowerCase() === "active" ||
          sprint.status === 2,
      ) ??
      sprintsQuery.sprints[0] ??
      null
    );
  }, [selectedSprintId, sprintsQuery.sprints]);

  const report = useMemo(() => {
    if (!selectedSprint) {
      return null;
    }

    const workItems = myWorkQuery.items.filter(
      (item) => item.sprintId === selectedSprint.sprintId,
    );

    const activeItems = workItems.filter(
      (item) =>
        statusValue(item.status) !== WorkItemStatus.Cancelled,
    );

    const completedItems = activeItems.filter(
      (item) => statusValue(item.status) === WorkItemStatus.Done,
    );

    const plannedHours = activeItems.reduce(
      (total, item) => total + (item.estimateHours ?? 0),
      0,
    );

    const completedHours = completedItems.reduce(
      (total, item) => total + (item.estimateHours ?? 0),
      0,
    );

    const remainingHours = Math.max(
      0,
      plannedHours - completedHours,
    );

    const completionPercentage = activeItems.length
      ? Math.round(
          (completedItems.length / activeItems.length) * 100,
        )
      : 0;

    const remainingDays = daysRemaining(selectedSprint.endDate);
    const today = new Date();
today.setHours(0, 0, 0, 0);

const sprintStartDate = localDate(selectedSprint.startDate);
const sprintEndDate = localDate(selectedSprint.endDate);

const totalSprintDays = Math.max(
  1,
  Math.round(
    (sprintEndDate.getTime() - sprintStartDate.getTime()) /
      86_400_000,
  ) + 1,
);

const elapsedSprintDays = Math.min(
  totalSprintDays,
  Math.max(
    0,
    Math.round(
      (today.getTime() - sprintStartDate.getTime()) /
        86_400_000,
    ) + 1,
  ),
);

const expectedCompletion = Math.round(
  (elapsedSprintDays / totalSprintDays) * 100,
);

const overdueItems = activeItems.filter((item) => {
  if (!item.dueDate) {
    return false;
  }

  const dueDate = new Date(item.dueDate);
  dueDate.setHours(0, 0, 0, 0);

  return (
    dueDate < today &&
    statusValue(item.status) !== WorkItemStatus.Done
  );
});

let health: {
  status: "on-track" | "at-risk" | "off-track";
  label: string;
  description: string;
};

if (
  remainingDays < 0 &&
  completedItems.length < activeItems.length
) {
  health = {
    status: "off-track",
    label: "Off track",
    description: `${activeItems.length - completedItems.length} unfinished work item${
      activeItems.length - completedItems.length === 1 ? "" : "s"
    } after the sprint end date.`,
  };
} else if (
  overdueItems.length > 0 ||
  completionPercentage + 15 < expectedCompletion
) {
  health = {
    status: "at-risk",
    label: "At risk",
    description:
      overdueItems.length > 0
        ? `${overdueItems.length} overdue work item${
            overdueItems.length === 1 ? "" : "s"
          } need attention.`
        : `${completionPercentage}% complete; ${expectedCompletion}% is expected by today.`,
  };
} else {
  health = {
    status: "on-track",
    label: "On track",
    description:
      activeItems.length === 0
        ? "No active work items have been added yet."
        : `${completionPercentage}% complete with ${
            Math.max(remainingDays, 0)
          } day${Math.max(remainingDays, 0) === 1 ? "" : "s"} remaining.`,
  };
}


    const sprintStart = localDate(selectedSprint.startDate);
const sprintEnd = localDate(selectedSprint.endDate);


today.setHours(23, 59, 59, 999);

const sprintDays = Math.max(
  1,
  Math.round(
    (sprintEnd.getTime() - sprintStart.getTime()) /
      86_400_000,
  ) + 1,
);

const burndownPoints: BurndownPoint[] = Array.from(
  { length: sprintDays },
  (_, index) => {
    const date = new Date(sprintStart);
    date.setDate(date.getDate() + index);
    date.setHours(23, 59, 59, 999);

    const ideal =
      sprintDays === 1
        ? 0
        : Math.max(
            0,
            plannedHours *
              (1 - index / (sprintDays - 1)),
          );

    const actual =
      date > today
        ? null
        : Math.max(
            0,
            plannedHours -
              completedItems
                .filter((item) => {
                  if (!item.updatedOnUtc) {
                    return false;
                  }

                  return (
                    new Date(item.updatedOnUtc).getTime() <=
                    date.getTime()
                  );
                })
                .reduce(
                  (total, item) =>
                    total + (item.estimateHours ?? 0),
                  0,
                ),
          );

    return {
      date,
      ideal,
      actual,
    };
  },
);
    const totalDays = daysBetween(
      selectedSprint.startDate,
      selectedSprint.endDate,
    );

    const statusItems = [
      {
        label: "To do",
        value: workItems.filter(
          (item) =>
            statusValue(item.status) === WorkItemStatus.Todo,
        ).length,
        color: "bg-slate-400",
      },
      {
        label: "In progress",
        value: workItems.filter(
          (item) =>
            statusValue(item.status) ===
            WorkItemStatus.InProgress,
        ).length,
        color: "bg-sky-500",
      },
      {
        label: "In review",
        value: workItems.filter(
          (item) =>
            statusValue(item.status) ===
            WorkItemStatus.InReview,
        ).length,
        color: "bg-violet-500",
      },
      {
        label: "Testing",
        value: workItems.filter(
          (item) =>
            statusValue(item.status) === WorkItemStatus.Testing,
        ).length,
        color: "bg-amber-500",
      },
      {
        label: "Done",
        value: completedItems.length,
        color: "bg-emerald-500",
      },
    ];

    return {
      workItems,
      completedItems,
      plannedHours,
      completedHours,
      remainingHours,
      completionPercentage,
      remainingDays,
      totalDays,
      statusItems,
      burndownPoints,
      overdueItems,
health,
    };
  }, [myWorkQuery.items, selectedSprint]);

  const visibleWorkItems = useMemo(() => {
  if (!report) {
    return [];
  }

  const search = workItemSearch.trim().toLowerCase();

  return report.workItems.filter((item) => {
    const matchesSearch =
      !search ||
      item.title.toLowerCase().includes(search) ||
      item.key.toLowerCase().includes(search) ||
      item.projectName.toLowerCase().includes(search);

    const matchesStatus =
      workItemStatus === "all" ||
      statusValue(item.status) === workItemStatus;

    return matchesSearch && matchesStatus;
  });
}, [report, workItemSearch, workItemStatus]);

const selectedWorkItem =
  report?.workItems.find(
    (item) => item.id === selectedWorkItemId,
  ) ?? null;

const selectedProjectQuery = useProject(
  selectedWorkItem?.projectId,
);

  function downloadCsv() {
    if (!selectedSprint || !report) {
      return;
    }

    const headers = [
      "Sprint",
      "Key",
      "Title",
      "Status",
      "Estimate hours",
      "Due date",
    ];

    const rows = report.workItems.map((item) => [
      selectedSprint.name,
      item.key,
      item.title,
      String(item.status),
      item.estimateHours ?? 0,
      item.dueDate ?? "",
    ]);

    const csv = [
      headers.map(csvCell).join(","),
      ...rows.map((row) => row.map(csvCell).join(",")),
    ].join("\n");

    const blob = new Blob([csv], {
      type: "text/csv;charset=utf-8;",
    });

    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");

    link.href = url;
    link.download = `${selectedSprint.name
      .toLowerCase()
      .replace(/\s+/g, "-")}-report.csv`;

    document.body.appendChild(link);
    link.click();
    link.remove();
    URL.revokeObjectURL(url);
  }

  const isLoading =
    sprintsQuery.isLoading || myWorkQuery.isLoading;

  return (
    <div className="mx-auto w-full max-w-7xl space-y-6">
      <div className="flex flex-col gap-4 sm:flex-row sm:items-end sm:justify-between">
        <div>
          <p className="text-sm font-medium text-[var(--devflow-primary)]">
            Reports
          </p>

          <h1 className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
            Sprint performance
          </h1>

          <p className="mt-1.5 text-sm text-slate-500">
            Track sprint delivery, scope, and estimated velocity.
          </p>
        </div>

        <Button
          type="button"
          variant="outline"
          disabled={!selectedSprint || !report}
          onClick={downloadCsv}
        >
          <Download className="h-4 w-4" />
          Export CSV
        </Button>
      </div>

      <section className="rounded-2xl border border-slate-200 bg-white p-4 shadow-sm">
        <label className="block max-w-xl">
          <span className="text-xs font-semibold uppercase tracking-wider text-slate-400">
            Sprint
          </span>

          <select
            value={selectedSprint?.sprintId ?? ""}
            onChange={(event) =>
              setSelectedSprintId(event.target.value)
            }
            className="mt-2 h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
          >
            {sprintsQuery.sprints.length === 0 && (
              <option value="">No sprints available</option>
            )}

            {sprintsQuery.sprints.map((sprint) => (
              <option
                key={sprint.sprintId}
                value={sprint.sprintId}
              >
                {sprint.projectKey} · {sprint.name}
              </option>
            ))}
          </select>
        </label>
      </section>

       {selectedWorkItem && selectedProjectQuery.data && (
        <WorkItemDetailDialog
          open
          onOpenChange={(open) => {
            if (!open) {
              setSelectedWorkItemId(null);
              void myWorkQuery.refetch();
            }
          }}
          projectId={selectedWorkItem.projectId}
          workItem={selectedWorkItem}
          members={selectedProjectQuery.data.members ?? []}
        />
      )}

      {isLoading && (
        <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
          {[0, 1, 2, 3].map((index) => (
            <div
              key={index}
              className="h-32 animate-pulse rounded-2xl bg-slate-100"
            />
          ))}
        </div>
      )}

      {!isLoading && !selectedSprint && (
        <section className="rounded-2xl border border-slate-200 bg-white px-5 py-16 text-center shadow-sm">
          <Rocket className="mx-auto h-8 w-8 text-slate-400" />

          <h2 className="mt-3 text-base font-semibold text-slate-900">
            No sprints available
          </h2>

          <p className="mt-1 text-sm text-slate-500">
            Create a sprint in a project to view its performance report.
          </p>
        </section>
      )}

      {!isLoading && selectedSprint && report && (
        <>
          <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
            <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
              <div>
                <p className="text-xs font-semibold uppercase tracking-wider text-slate-400">
                  {selectedSprint.projectName}
                </p>

                <h2 className="mt-1 text-lg font-semibold text-slate-900">
                  {selectedSprint.name}
                                </h2>
                                <div
                className={`mt-3 inline-flex items-center gap-2 rounded-lg px-3 py-2 text-xs font-medium ${
                    report.health.status === "on-track"
                    ? "bg-emerald-50 text-emerald-700"
                    : report.health.status === "at-risk"
                        ? "bg-amber-50 text-amber-700"
                        : "bg-red-50 text-red-700"
                }`}
                >
                {report.health.status === "on-track" ? (
                    <CheckCircle2 className="h-4 w-4" />
                ) : (
                    <AlertTriangle className="h-4 w-4" />
                )}

                <span>{report.health.label}</span>

                <span className="h-1 w-1 rounded-full bg-current opacity-50" />

                <span className="font-normal">
                    {report.health.description}
                </span>
                </div>
                {selectedSprint.goal && (
                  <p className="mt-1 text-sm text-slate-500">
                    {selectedSprint.goal}
                  </p>
                )}
              </div>

              <div className="rounded-lg bg-slate-50 px-3 py-2 text-right">
                <p className="text-xs text-slate-400">Sprint period</p>
                <p className="mt-1 text-sm font-medium text-slate-700">
                  {new Intl.DateTimeFormat(undefined, {
                    day: "numeric",
                    month: "short",
                  }).format(new Date(selectedSprint.startDate))}
                  {" – "}
                  {new Intl.DateTimeFormat(undefined, {
                    day: "numeric",
                    month: "short",
                  }).format(new Date(selectedSprint.endDate))}
                </p>
              </div>
            </div>
          </section>

          <section className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
            <SprintCard
              icon={ListTodo}
              iconClassName="bg-sky-50 text-sky-600"
              label="Planned work"
              value={report.workItems.length}
              description={`${formatHours(report.plannedHours)} estimated`}
            />

            <SprintCard
              icon={CheckCircle2}
              iconClassName="bg-emerald-50 text-emerald-600"
              label="Completion"
              value={`${report.completionPercentage}%`}
              description={`${report.completedItems.length} items completed`}
            />

            <SprintCard
              icon={Gauge}
              iconClassName="bg-violet-50 text-violet-600"
              label="Velocity"
              value={formatHours(report.completedHours)}
              description="Completed estimated work"
            />

            <SprintCard
              icon={AlertTriangle}
              iconClassName={
                report.remainingDays < 0
                  ? "bg-red-50 text-red-600"
                  : "bg-amber-50 text-amber-600"
              }
              label="Time remaining"
              value={
                report.remainingDays < 0
                  ? `${Math.abs(report.remainingDays)}d overdue`
                  : `${report.remainingDays}d`
              }
              description={`${formatHours(report.remainingHours)} remaining`}
            />
          </section>

          <div className="grid gap-6 xl:grid-cols-[1.15fr_1fr]">
            <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
              <h2 className="text-sm font-semibold text-slate-900">
                Sprint progress
              </h2>

              <p className="mt-1 text-xs text-slate-500">
                Completion based on work items marked done.
              </p>

              <div className="mt-6">
                <div className="flex items-end justify-between gap-3">
                  <p className="text-4xl font-semibold tracking-tight text-slate-900">
                    {report.completionPercentage}%
                  </p>

                  <p className="text-sm text-slate-500">
                    {report.completedItems.length} of{" "}
                    {report.workItems.length} items
                  </p>
                </div>

                <div className="mt-3 h-3 overflow-hidden rounded-full bg-slate-100">
                  <div
                    className="h-full rounded-full bg-emerald-500 transition-all"
                    style={{
                      width: `${report.completionPercentage}%`,
                    }}
                  />
                </div>

                <div className="mt-6 grid grid-cols-2 gap-4 border-t border-slate-100 pt-5">
                  <div>
                    <p className="text-xs text-slate-400">
                      Planned estimate
                    </p>

                    <p className="mt-1 text-lg font-semibold text-slate-800">
                      {formatHours(report.plannedHours)}
                    </p>
                  </div>

                  <div>
                    <p className="text-xs text-slate-400">
                      Remaining estimate
                    </p>

                    <p className="mt-1 text-lg font-semibold text-slate-800">
                      {formatHours(report.remainingHours)}
                    </p>
                  </div>
                </div>
              </div>
            </section>

            <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
              <h2 className="text-sm font-semibold text-slate-900">
                Work status
              </h2>

              <p className="mt-1 text-xs text-slate-500">
                Current distribution of sprint work.
              </p>

              <div className="mt-5 space-y-4">
                {report.statusItems.map((item) => {
                  const percentage = report.workItems.length
                    ? Math.round(
                        (item.value / report.workItems.length) * 100,
                      )
                    : 0;

                  return (
                    <div key={item.label}>
                      <div className="flex justify-between text-xs">
                        <span className="font-medium text-slate-600">
                          {item.label}
                        </span>

                        <span className="text-slate-400">
                          {item.value} · {percentage}%
                        </span>
                      </div>

                      <div className="mt-2 h-2 overflow-hidden rounded-full bg-slate-100">
                        <div
                          className={`h-full rounded-full ${item.color}`}
                          style={{ width: `${percentage}%` }}
                        />
                      </div>
                    </div>
                  );
                })}
              </div>
            </section>

            <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
                <div className="flex items-start gap-3">
                    <div className="flex h-9 w-9 items-center justify-center rounded-xl bg-sky-50 text-sky-600">
                    <TrendingDown className="h-4 w-4" />
                    </div>

                    <div>
                    <h2 className="text-sm font-semibold text-slate-900">
                        Burndown
                    </h2>

                    <p className="mt-1 text-xs text-slate-500">
                        Remaining estimated hours across the sprint. Actual progress
                        is estimated from completed work-item update dates.
                    </p>
                    </div>
                </div>

                <div className="mt-5">
                    <SprintBurndownChart points={report.burndownPoints} />
                </div>
            </section>

            <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
                <div className="border-b border-slate-100 px-5 py-4">
                    <h2 className="text-sm font-semibold text-slate-900">
                    Sprint work items
                    </h2>

                    <p className="mt-1 text-xs text-slate-500">
                    Review the work currently included in this sprint.
                    </p>

                    <div className="mt-4 flex flex-col gap-3 sm:flex-row">
                    <div className="relative flex-1">
                        <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />

                        <Input
                        type="search"
                        value={workItemSearch}
                        placeholder="Search by title, key, or project..."
                        className="pl-9"
                        onChange={(event) =>
                            setWorkItemSearch(event.target.value)
                        }
                        />
                    </div>

                    <select
                        value={workItemStatus}
                        onChange={(event) =>
                        setWorkItemStatus(
                            event.target.value === "all"
                            ? "all"
                            : (Number(event.target.value) as WorkItemStatus),
                        )
                        }
                        className="h-10 rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                    >
                        <option value="all">All statuses</option>
                        <option value={WorkItemStatus.Todo}>To do</option>
                        <option value={WorkItemStatus.InProgress}>
                        In progress
                        </option>
                        <option value={WorkItemStatus.InReview}>
                        In review
                        </option>
                        <option value={WorkItemStatus.Testing}>Testing</option>
                        <option value={WorkItemStatus.Done}>Done</option>
                        <option value={WorkItemStatus.Cancelled}>
                        Cancelled
                        </option>
                    </select>
                    </div>
                </div>

                {visibleWorkItems.length === 0 ? (
                    <div className="px-5 py-14 text-center">
                    <ListTodo className="mx-auto h-7 w-7 text-slate-400" />

                    <p className="mt-3 text-sm font-medium text-slate-700">
                        No work items found
                    </p>

                    <p className="mt-1 text-xs text-slate-400">
                        Try changing your search or status filter.
                    </p>
                    </div>
                ) : (
                    <div className="overflow-x-auto">
                    <table className="w-full min-w-[760px] text-left">
                        <thead className="border-b border-slate-100 bg-slate-50 text-xs text-slate-500">
                        <tr>
                            <th className="px-5 py-3 font-medium">Work item</th>
                            <th className="px-4 py-3 font-medium">Status</th>
                            <th className="px-4 py-3 font-medium">Priority</th>
                            <th className="px-4 py-3 font-medium">Assignee</th>
                            <th className="px-4 py-3 font-medium">Estimate</th>
                            <th className="px-5 py-3 text-right font-medium">
                            Due date
                            </th>
                        </tr>
                        </thead>

                        <tbody className="divide-y divide-slate-100">
                        {visibleWorkItems.map((item) => (
                            <tr
                            key={item.id}
                            className="transition-colors hover:bg-slate-50"
                            >
                            <td className="px-5 py-4">
                                <Link
                                to={`/projects/${item.projectId}/work-items/${item.id}`}
                                className="block max-w-sm"
                                >
                                <p className="truncate text-sm font-medium text-slate-800 transition-colors hover:text-[var(--devflow-primary)]">
                                    {item.title}
                                </p>

                                <p className="mt-1 text-xs text-slate-500">
                                    <span className="font-medium text-slate-600">
                                    {item.key}
                                    </span>
                                    {" · "}
                                    {item.projectName}
                                </p>
                                </Link>
                            </td>

                            <td className="px-4 py-4">
                                <span className="rounded-full bg-slate-100 px-2.5 py-1 text-xs font-medium text-slate-600">
                                {statusLabel(item.status)}
                                </span>
                            </td>

                            <td className="px-4 py-4">
                                <span
                                className={`rounded-full px-2.5 py-1 text-xs font-medium ${priorityClass(
                                    item.priority,
                                )}`}
                                >
                                {priorityLabel(item.priority)}
                                </span>
                            </td>

                            <td className="px-4 py-4 text-sm text-slate-600">
                                {item.assigneeId
                                ? `Member ${item.assigneeId.slice(0, 8)}`
                                : "Unassigned"}
                            </td>

                            <td className="px-4 py-4 text-sm font-medium text-slate-600">
                                {item.estimateHours
                                ? formatHours(item.estimateHours)
                                : "—"}
                            </td>

                            <td className="px-5 py-4 text-right text-sm text-slate-600">
                                {item.dueDate
                                ? new Intl.DateTimeFormat(undefined, {
                                    day: "numeric",
                                    month: "short",
                                    year: "numeric",
                                    }).format(new Date(item.dueDate))
                                : "—"}
                            </td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                    </div>
                )}
                </section>
          </div>
        </>
      )}
    </div>
  );
}

function SprintCard({
  icon: Icon,
  iconClassName,
  label,
  value,
  description,
}: {
  icon: React.ComponentType<{ className?: string }>;
  iconClassName: string;
  label: string;
  value: string | number;
  description: string;
}) {
  return (
    <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
      <div className="flex items-start justify-between gap-4">
        <div>
          <p className="text-sm font-medium text-slate-500">{label}</p>
          <p className="mt-2 text-2xl font-semibold tracking-tight text-slate-900">
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

function SprintBurndownChart({
  points,
}: {
  points: BurndownPoint[];
}) {
  const width = 800;
  const height = 260;
  const padding = {
    top: 18,
    right: 20,
    bottom: 34,
    left: 42,
  };

  const maximum = Math.max(
    ...points.flatMap((point) => [
      point.ideal,
      point.actual ?? 0,
    ]),
    1,
  );

  const chartWidth = width - padding.left - padding.right;
  const chartHeight = height - padding.top - padding.bottom;

  function x(index: number) {
    if (points.length <= 1) {
      return padding.left + chartWidth / 2;
    }

    return (
      padding.left +
      (index / (points.length - 1)) * chartWidth
    );
  }

  function y(value: number) {
    return (
      padding.top +
      chartHeight -
      (value / maximum) * chartHeight
    );
  }

  const idealPath = points
    .map(
      (point, index) =>
        `${index === 0 ? "M" : "L"} ${x(index)} ${y(
          point.ideal,
        )}`,
    )
    .join(" ");

  const actualPoints = points
    .map((point, index) => ({
      index,
      value: point.actual,
    }))
    .filter(
      (
        point,
      ): point is {
        index: number;
        value: number;
      } => point.value !== null,
    );

  const actualPath = actualPoints
    .map(
      (point, index) =>
        `${index === 0 ? "M" : "L"} ${x(point.index)} ${y(
          point.value,
        )}`,
    )
    .join(" ");

  const labelIndexes = Array.from(
    new Set([
      0,
      Math.floor((points.length - 1) / 2),
      points.length - 1,
    ]),
  );

  return (
    <div>
      <div className="mb-4 flex flex-wrap items-center gap-4 text-xs text-slate-500">
        <span className="inline-flex items-center gap-2">
          <span className="h-2.5 w-2.5 rounded-full bg-slate-400" />
          Ideal remaining hours
        </span>

        <span className="inline-flex items-center gap-2">
          <span className="h-2.5 w-2.5 rounded-full bg-sky-500" />
          Actual remaining hours
        </span>
      </div>

      <div className="overflow-x-auto">
        <svg
          viewBox={`0 0 ${width} ${height}`}
          className="min-w-[620px] w-full"
          role="img"
          aria-label="Sprint burndown chart"
        >
          {[0, 0.25, 0.5, 0.75, 1].map((step) => {
            const lineY = padding.top + chartHeight * step;
            const value = Math.round(maximum * (1 - step));

            return (
              <g key={step}>
                <line
                  x1={padding.left}
                  x2={width - padding.right}
                  y1={lineY}
                  y2={lineY}
                  stroke="#e2e8f0"
                  strokeWidth="1"
                />

                <text
                  x={padding.left - 10}
                  y={lineY + 4}
                  textAnchor="end"
                  fontSize="11"
                  fill="#94a3b8"
                >
                  {value}h
                </text>
              </g>
            );
          })}

          <path
            d={idealPath}
            fill="none"
            stroke="#94a3b8"
            strokeWidth="2"
            strokeDasharray="5 5"
          />

          {actualPath && (
            <path
              d={actualPath}
              fill="none"
              stroke="#0ea5e9"
              strokeWidth="3"
              strokeLinecap="round"
              strokeLinejoin="round"
            />
          )}

          {actualPoints.map((point) => (
            <circle
              key={point.index}
              cx={x(point.index)}
              cy={y(point.value)}
              r="3.5"
              fill="#0ea5e9"
              stroke="white"
              strokeWidth="2"
            />
          ))}

          {labelIndexes.map((index) => (
            <text
              key={index}
              x={x(index)}
              y={height - 10}
              textAnchor="middle"
              fontSize="11"
              fill="#94a3b8"
            >
              {chartDateLabel(points[index].date)}
            </text>
          ))}
        </svg>
      </div>

      
    </div>
    
  );
}