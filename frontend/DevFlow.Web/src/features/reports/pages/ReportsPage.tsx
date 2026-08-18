import { useMemo, useState, type ComponentType } from "react";
import {
  AlertTriangle,
  BarChart3,
  CheckCircle2,
  Download,
  FolderKanban,
  ListTodo,
  Rocket,
  Users,
} from "lucide-react";
import { Link } from "react-router-dom";
import { Button } from "@/components/ui/button";
import {
  WorkItemPriority,
  WorkItemStatus,
} from "@/features/projects/api/project-resources-api";
import { useMyWork } from "@/features/projects/hooks/use-my-work";
import { useProjects } from "@/features/projects/hooks/use-projects";
import {
  useProjectReportSummary,
  useProjectVelocity,
  useProjectWorkload,
} from "../hooks/use-project-reports";

type StatusSummary = {
  label: string;
  value: number;
  color: string;
};

type PrioritySummary = {
  label: string;
  value: number;
  color: string;
};

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

function priorityValue(value: string | number) {
  if (typeof value === "number") {
    return value;
  }

  const priorities: Record<string, WorkItemPriority> = {
    lowest: WorkItemPriority.Lowest,
    low: WorkItemPriority.Low,
    medium: WorkItemPriority.Medium,
    high: WorkItemPriority.High,
    highest: WorkItemPriority.Highest,
  };

  return priorities[value.toLowerCase()] ?? WorkItemPriority.Medium;
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
  const labels: Record<number, string> = {
    [WorkItemPriority.Lowest]: "Lowest",
    [WorkItemPriority.Low]: "Low",
    [WorkItemPriority.Medium]: "Medium",
    [WorkItemPriority.High]: "High",
    [WorkItemPriority.Highest]: "Highest",
  };

  return labels[priorityValue(value)] ?? "Medium";
}

function csvCell(value: string | number | null | undefined) {
  const text = String(value ?? "");

  return `"${text.replace(/"/g, '""')}"`;
}

export function ReportsPage() {
  const projectsQuery = useProjects({
    page: 1,
    pageSize: 100,
  });
  const myWorkQuery = useMyWork();

  const [projectId, setProjectId] = useState("all");
  const selectedProjectId = projectId === "all" ? null : projectId;
  const summaryQuery = useProjectReportSummary(selectedProjectId);
  const velocityQuery = useProjectVelocity(selectedProjectId);
  const workloadQuery = useProjectWorkload(selectedProjectId);

  const report = useMemo(() => {
    const workItems = myWorkQuery.items.filter(
      (item) => projectId === "all" || item.projectId === projectId,
    );

    const actionableItems = workItems.filter(
      (item) =>
        statusValue(item.status) !== WorkItemStatus.Cancelled,
    );

    const completedItems = actionableItems.filter(
      (item) => statusValue(item.status) === WorkItemStatus.Done,
    );

    const overdueItems = actionableItems.filter((item) => {
      if (!item.dueDate) {
        return false;
      }

      return (
        new Date(item.dueDate) < new Date() &&
        statusValue(item.status) !== WorkItemStatus.Done
      );
    });

    const statusSummaries: StatusSummary[] = [
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

    const prioritySummaries: PrioritySummary[] = [
      {
        label: "Highest",
        value: workItems.filter(
          (item) =>
            priorityValue(item.priority) ===
            WorkItemPriority.Highest,
        ).length,
        color: "bg-red-500",
      },
      {
        label: "High",
        value: workItems.filter(
          (item) =>
            priorityValue(item.priority) === WorkItemPriority.High,
        ).length,
        color: "bg-orange-500",
      },
      {
        label: "Medium",
        value: workItems.filter(
          (item) =>
            priorityValue(item.priority) ===
            WorkItemPriority.Medium,
        ).length,
        color: "bg-amber-400",
      },
      {
        label: "Low",
        value: workItems.filter(
          (item) =>
            priorityValue(item.priority) === WorkItemPriority.Low,
        ).length,
        color: "bg-sky-400",
      },
      {
        label: "Lowest",
        value: workItems.filter(
          (item) =>
            priorityValue(item.priority) ===
            WorkItemPriority.Lowest,
        ).length,
        color: "bg-slate-400",
      },
    ];

    const workload = Object.values(
      workItems.reduce<
        Record<
          string,
          {
            assignee: string;
            assigned: number;
            completed: number;
            inProgress: number;
          }
        >
      >((result, item) => {
        const assignee = item.assigneeId
          ? `Member ${item.assigneeId.slice(0, 8)}`
          : "Unassigned";

        const current = result[assignee] ?? {
          assignee,
          assigned: 0,
          completed: 0,
          inProgress: 0,
        };

        current.assigned += 1;

        if (statusValue(item.status) === WorkItemStatus.Done) {
          current.completed += 1;
        }

        if (
          statusValue(item.status) === WorkItemStatus.InProgress ||
          statusValue(item.status) === WorkItemStatus.InReview ||
          statusValue(item.status) === WorkItemStatus.Testing
        ) {
          current.inProgress += 1;
        }

        result[assignee] = current;

        return result;
      }, {}),
    ).sort((left, right) => right.assigned - left.assigned);

    return {
      workItems,
      completedItems,
      overdueItems,
      completionPercentage: actionableItems.length
        ? Math.round(
            (completedItems.length / actionableItems.length) * 100,
          )
        : 0,
      statusSummaries,
      prioritySummaries,
      workload,
    };
  }, [myWorkQuery.items, projectId]);

  const summary = summaryQuery.data;
  const statusSummaries = summary
    ? [
        { label: "To do", value: summary.todoCount, color: "bg-slate-400" },
        { label: "In progress", value: summary.inProgressCount, color: "bg-sky-500" },
        { label: "In review", value: summary.reviewCount, color: "bg-violet-500" },
        { label: "Done", value: summary.doneCount, color: "bg-emerald-500" },
      ]
    : report.statusSummaries;
  const workload = workloadQuery.data
    ? workloadQuery.data.map((member) => ({
        assignee: `Member ${member.userId.slice(0, 8)}`,
        assigned: member.totalWorkItems,
        completed: 0,
        inProgress: 0,
        estimateHours: member.totalEstimateHours,
      }))
    : report.workload.map((member) => ({ ...member, estimateHours: undefined }));
  const totalWorkItems = summary?.totalWorkItems ?? report.workItems.length;
  const completedItems = summary?.doneCount ?? report.completedItems.length;
  const completionPercentage = totalWorkItems
    ? Math.round((completedItems / totalWorkItems) * 100)
    : 0;

  function downloadCsv() {
    const headers = [
      "Key",
      "Title",
      "Project",
      "Status",
      "Priority",
      "Assignee ID",
      "Due date",
      "Estimate hours",
    ];

    const rows = report.workItems.map((item) => [
      item.key,
      item.title,
      item.projectName,
      statusLabel(item.status),
      priorityLabel(item.priority),
      item.assigneeId,
      item.dueDate
        ? new Date(item.dueDate).toLocaleDateString()
        : "",
      item.estimateHours,
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
    link.download = `devflow-report-${
      projectId === "all" ? "workspace" : projectId
    }.csv`;

    document.body.appendChild(link);
    link.click();
    link.remove();
    URL.revokeObjectURL(url);
  }

  const isLoading = projectsQuery.isLoading || myWorkQuery.isLoading ||
    Boolean(selectedProjectId && (summaryQuery.isLoading || velocityQuery.isLoading || workloadQuery.isLoading));
  const reportError = Boolean(selectedProjectId && (summaryQuery.isError || velocityQuery.isError || workloadQuery.isError));

  return (
    <div className="mx-auto w-full max-w-7xl space-y-6">
      <div className="flex flex-col gap-4 sm:flex-row sm:items-end sm:justify-between">
        <div>
          <p className="text-sm font-medium text-[var(--devflow-primary)]">
            Workspace
          </p>

          <h1 className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
            Reports
          </h1>

          <p className="mt-1.5 text-sm text-slate-500">
            Understand delivery progress and team workload.
          </p>
        </div>

        <div className="flex flex-wrap gap-2">
            <Button type="button" variant="outline" asChild>
                <Link to="/reports/sprints">
                <Rocket className="h-4 w-4" />
                Sprint reports
                </Link>
            </Button>

            <Button
                type="button"
                variant="outline"
                disabled={report.workItems.length === 0}
                onClick={downloadCsv}
            >
                <Download className="h-4 w-4" />
                Export CSV
            </Button>
            </div>
      </div>

      <section className="rounded-2xl border border-slate-200 bg-white p-4 shadow-sm">
        <label className="block max-w-md">
          <span className="text-xs font-semibold uppercase tracking-wider text-slate-400">
            Project
          </span>

          <select
            value={projectId}
            onChange={(event) => setProjectId(event.target.value)}
            className="mt-2 h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
          >
            <option value="all">All projects</option>

            {(projectsQuery.data?.items ?? []).map((project) => (
              <option
                key={project.projectId}
                value={project.projectId}
              >
                {project.name} ({project.key})
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

      {reportError && (
        <section className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
          Unable to load the server report for this project. Please try again.
        </section>
      )}

      {!isLoading && !reportError && (
        <>
          <section className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
            <ReportCard
              icon={ListTodo}
              iconClassName="bg-sky-50 text-sky-600"
              label="Total work items"
              value={totalWorkItems}
              description="Items in the selected report"
            />

            <ReportCard
              icon={CheckCircle2}
              iconClassName="bg-emerald-50 text-emerald-600"
              label="Completion"
              value={`${completionPercentage}%`}
              description={`${completedItems} completed items`}
            />

            <ReportCard
              icon={AlertTriangle}
              iconClassName="bg-red-50 text-red-600"
              label="Overdue"
              value={report.overdueItems.length}
              description="Incomplete items past due"
            />

            <ReportCard
              icon={Users}
              iconClassName="bg-violet-50 text-violet-600"
              label="Contributors"
              value={summary?.totalMembers ?? workload.length}
              description="Members with assigned work"
            />
          </section>

          <div className="grid gap-6 xl:grid-cols-2">
            <ReportBreakdown
              title="Work status"
              description="Distribution of work by current status."
              items={statusSummaries}
              total={totalWorkItems}
            />

            <ReportBreakdown
              title="Priority distribution"
              description="How work is prioritised in this selection."
              items={report.prioritySummaries}
              total={report.workItems.length}
            />
          </div>

          {selectedProjectId && (velocityQuery.data?.length ?? 0) > 0 && (
            <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
              <div className="border-b border-slate-100 px-5 py-4">
                <h2 className="text-sm font-semibold text-slate-900">
                  Sprint velocity
                </h2>
                <p className="mt-1 text-xs text-slate-500">
                  Completed work compared with the sprint commitment.
                </p>
              </div>

              <div className="divide-y divide-slate-100">
                {velocityQuery.data?.map((sprint) => {
                  const completion = sprint.committed
                    ? Math.round((sprint.completed / sprint.committed) * 100)
                    : 0;

                  return (
                    <div key={sprint.sprintId} className="grid gap-3 px-5 py-4 sm:grid-cols-[minmax(0,1fr)_110px_1fr] sm:items-center">
                      <p className="truncate text-sm font-medium text-slate-800">
                        {sprint.sprintName}
                      </p>
                      <p className="text-sm text-slate-500">
                        {sprint.completed}/{sprint.committed} done
                      </p>
                      <div className="h-2 overflow-hidden rounded-full bg-slate-100">
                        <div className="h-full rounded-full bg-blue-500" style={{ width: `${Math.min(completion, 100)}%` }} />
                      </div>
                    </div>
                  );
                })}
              </div>
            </section>
          )}

          <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
            <div className="flex items-start gap-3 border-b border-slate-100 px-5 py-4">
              <div className="flex h-9 w-9 items-center justify-center rounded-xl bg-slate-50 text-slate-500">
                <Users className="h-4 w-4" />
              </div>

              <div>
                <h2 className="text-sm font-semibold text-slate-900">
                  Team workload
                </h2>

                <p className="mt-1 text-xs text-slate-500">
                  Assigned items and delivery progress per team member.
                </p>
              </div>
            </div>

            {workload.length === 0 ? (
              <div className="px-5 py-14 text-center">
                <FolderKanban className="mx-auto h-7 w-7 text-slate-400" />

                <p className="mt-3 text-sm font-medium text-slate-700">
                  No workload data
                </p>

                <p className="mt-1 text-xs text-slate-400">
                  Assign work items to see team capacity here.
                </p>
              </div>
            ) : (
              <div className="divide-y divide-slate-100">
                {workload.map((member) => {
                  const completion = member.assigned
                    ? Math.round(
                        (member.completed / member.assigned) * 100,
                      )
                    : 0;

                  return (
                    <div
                      key={member.assignee}
                      className="grid gap-4 px-5 py-4 md:grid-cols-[minmax(0,1fr)_120px_120px_160px]"
                    >
                      <div className="min-w-0">
                        <p className="truncate text-sm font-medium text-slate-800">
                          {member.assignee}
                        </p>

                        <p className="mt-1 text-xs text-slate-500">
                          {member.assigned} assigned work item
                          {member.assigned === 1 ? "" : "s"}
                        </p>
                      </div>

                      <div>
                        <p className="text-xs text-slate-400">
                          In progress
                        </p>

                        <p className="mt-1 text-sm font-semibold text-slate-700">
                          {member.inProgress || "—"}
                        </p>
                      </div>

                      <div>
                        <p className="text-xs text-slate-400">
                          Completed
                        </p>

                        <p className="mt-1 text-sm font-semibold text-slate-700">
                          {member.completed || "—"}
                        </p>
                      </div>

                      <div>
                        <div className="flex items-center justify-between text-xs">
                          <span className="text-slate-400">
                            Completion
                          </span>

                          <span className="font-semibold text-slate-600">
                            {member.estimateHours
                              ? `${member.estimateHours}h estimated`
                              : `${completion}%`}
                          </span>
                        </div>

                        <div className="mt-2 h-2 overflow-hidden rounded-full bg-slate-100">
                          <div
                            className="h-full rounded-full bg-emerald-500"
                            style={{ width: `${completion}%` }}
                          />
                        </div>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </section>
        </>
      )}
    </div>
  );
}

function ReportCard({
  icon: Icon,
  iconClassName,
  label,
  value,
  description,
}: {
  icon: ComponentType<{ className?: string }>;
  iconClassName: string;
  label: string;
  value: number | string;
  description: string;
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

function ReportBreakdown({
  title,
  description,
  items,
  total,
}: {
  title: string;
  description: string;
  items: Array<StatusSummary | PrioritySummary>;
  total: number;
}) {
  const largestValue = Math.max(
    ...items.map((item) => item.value),
    1,
  );

  return (
    <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
      <h2 className="text-sm font-semibold text-slate-900">{title}</h2>

      <p className="mt-1 text-xs text-slate-500">{description}</p>

      <div className="mt-5 space-y-4">
        {items.map((item) => {
          const percentage = total
            ? Math.round((item.value / total) * 100)
            : 0;

          return (
            <div key={item.label}>
              <div className="flex items-center justify-between gap-3 text-xs">
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
                  style={{
                    width: `${(item.value / largestValue) * 100}%`,
                  }}
                />
              </div>
            </div>
          );
        })}
      </div>
    </section>
  );
}
