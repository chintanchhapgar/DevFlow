import { useMemo, useState } from "react";
import {
  AlertTriangle,
  CheckCircle2,
  Download,
  Gauge,
  ListTodo,
  Rocket,
} from "lucide-react";

import { Button } from "@/components/ui/button";
import { WorkItemStatus } from "@/features/projects/api/project-resources-api";
import { useMyWork } from "@/features/projects/hooks/use-my-work";

import { useProjectSprints } from "../hooks/use-sprint-reports";

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

export function SprintReportsPage() {
  const sprintsQuery = useProjectSprints();
  const myWorkQuery = useMyWork();

  const [selectedSprintId, setSelectedSprintId] = useState("");

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
    };
  }, [myWorkQuery.items, selectedSprint]);

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