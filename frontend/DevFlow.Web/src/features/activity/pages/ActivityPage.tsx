import { useMemo, useState } from "react";
import {
  Activity,
  CheckCircle2,
  CircleDot,
  Clock3,
  Search,
} from "lucide-react";
import { Link } from "react-router-dom";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { WorkItemStatus } from "@/features/projects/api/project-resources-api";
import { useMyWork } from "@/features/projects/hooks/use-my-work";

type ActivityFilter = "all" | "updated" | "completed";

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

function formatActivityDate(value?: string | null) {
  if (!value) {
    return "Recently";
  }

  const date = new Date(value);
  const now = new Date();
  const dayStart = new Date(now.getFullYear(), now.getMonth(), now.getDate());
  const dateStart = new Date(
    date.getFullYear(),
    date.getMonth(),
    date.getDate(),
  );
  const daysAgo = Math.round(
    (dayStart.getTime() - dateStart.getTime()) / 86_400_000,
  );

  if (daysAgo === 0) return "Today";
  if (daysAgo === 1) return "Yesterday";
  if (daysAgo > 1 && daysAgo < 7) return `${daysAgo} days ago`;

  return new Intl.DateTimeFormat(undefined, {
    day: "numeric",
    month: "short",
    year: date.getFullYear() === now.getFullYear() ? undefined : "numeric",
  }).format(date);
}

export function ActivityPage() {
  const myWorkQuery = useMyWork();
  const [search, setSearch] = useState("");
  const [filter, setFilter] = useState<ActivityFilter>("all");

  const activities = useMemo(() => {
    const searchTerm = search.trim().toLowerCase();

    return myWorkQuery.items
      .filter((item) => {
        const isCompleted = statusValue(item.status) === WorkItemStatus.Done;
        const matchesFilter =
          filter === "all" ||
          (filter === "completed" ? isCompleted : !isCompleted);
        const matchesSearch =
          !searchTerm ||
          item.title.toLowerCase().includes(searchTerm) ||
          item.key.toLowerCase().includes(searchTerm) ||
          item.projectName.toLowerCase().includes(searchTerm);

        return matchesFilter && matchesSearch;
      })
      .sort((left, right) => {
        const leftDate = new Date(
          left.updatedOnUtc ?? left.createdOnUtc ?? 0,
        ).getTime();
        const rightDate = new Date(
          right.updatedOnUtc ?? right.createdOnUtc ?? 0,
        ).getTime();

        return rightDate - leftDate;
      });
  }, [filter, myWorkQuery.items, search]);

  return (
    <div className="mx-auto w-full max-w-5xl space-y-6">
      <div>
        <p className="text-sm font-medium text-[var(--devflow-primary)]">
          Workspace
        </p>
        <h1 className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
          Activity
        </h1>
        <p className="mt-1.5 text-sm text-slate-500">
          Keep up with recent changes across the projects you can access.
        </p>
      </div>

      <section className="rounded-2xl border border-slate-200 bg-white p-4 shadow-sm">
        <div className="flex flex-col gap-3 sm:flex-row sm:items-center">
          <div className="relative flex-1">
            <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
            <Input
              type="search"
              value={search}
              placeholder="Search activity..."
              className="pl-9"
              onChange={(event) => setSearch(event.target.value)}
            />
          </div>
          <div className="flex rounded-lg border border-slate-200 bg-slate-50 p-1">
            {([
              ["all", "All"],
              ["updated", "Updated"],
              ["completed", "Completed"],
            ] as const).map(([value, label]) => (
              <button
                key={value}
                type="button"
                onClick={() => setFilter(value)}
                className={`rounded-md px-3 py-1.5 text-xs font-medium transition-colors ${
                  filter === value
                    ? "bg-white text-slate-900 shadow-sm"
                    : "text-slate-500 hover:text-slate-800"
                }`}
              >
                {label}
              </button>
            ))}
          </div>
        </div>
      </section>

      {myWorkQuery.isLoading && (
        <div className="space-y-3">
          {[0, 1, 2, 3].map((index) => (
            <div key={index} className="h-20 animate-pulse rounded-xl bg-slate-100" />
          ))}
        </div>
      )}

      {myWorkQuery.isError && (
        <section className="rounded-2xl border border-red-200 bg-red-50 p-5">
          <p className="font-medium text-red-800">Unable to load activity.</p>
          <p className="mt-1 text-sm text-red-700">Please try again.</p>
          <Button className="mt-4" variant="outline" size="sm" onClick={() => myWorkQuery.refetch()}>
            Try again
          </Button>
        </section>
      )}

      {!myWorkQuery.isLoading && !myWorkQuery.isError && activities.length === 0 && (
        <section className="flex min-h-72 flex-col items-center justify-center rounded-2xl border border-slate-200 bg-white px-5 text-center shadow-sm">
          <Activity className="h-8 w-8 text-slate-400" />
          <h2 className="mt-3 text-base font-semibold text-slate-900">No activity found</h2>
          <p className="mt-1 text-sm text-slate-500">
            {search || filter !== "all"
              ? "Try changing or clearing your filters."
              : "Updates from work items will appear here."}
          </p>
          {(search || filter !== "all") && (
            <Button
              type="button"
              variant="outline"
              size="sm"
              className="mt-4"
              onClick={() => {
                setSearch("");
                setFilter("all");
              }}
            >
              Clear filters
            </Button>
          )}
        </section>
      )}

      {!myWorkQuery.isLoading && !myWorkQuery.isError && activities.length > 0 && (
        <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
          <div className="border-b border-slate-100 px-5 py-4">
            <p className="text-sm font-semibold text-slate-900">Recent updates</p>
            <p className="mt-1 text-xs text-slate-500">{activities.length} work item{activities.length === 1 ? "" : "s"}</p>
          </div>
          <div className="divide-y divide-slate-100">
            {activities.map((item) => {
              const completed = statusValue(item.status) === WorkItemStatus.Done;
              const timestamp = item.updatedOnUtc ?? item.createdOnUtc;
              const Icon = completed ? CheckCircle2 : CircleDot;

              return (
                <Link
                  key={item.id}
                  to={`/projects/${item.projectId}`}
                  className="flex gap-3 px-5 py-4 transition-colors hover:bg-slate-50"
                >
                  <div className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-xl ${completed ? "bg-emerald-50 text-emerald-600" : "bg-sky-50 text-sky-600"}`}>
                    <Icon className="h-4 w-4" />
                  </div>
                  <div className="min-w-0 flex-1">
                    <div className="flex items-start justify-between gap-3">
                      <p className="truncate text-sm font-medium text-slate-800">
                        {completed ? "Completed" : "Updated"} <span className="font-normal">{item.title}</span>
                      </p>
                      <span className="shrink-0 text-[11px] font-medium text-slate-400">{formatActivityDate(timestamp)}</span>
                    </div>
                    <div className="mt-1 flex flex-wrap items-center gap-x-2 gap-y-1 text-xs text-slate-500">
                      <span className="font-medium text-slate-600">{item.key}</span>
                      <span className="text-slate-300">•</span>
                      <span>{item.projectName}</span>
                      {timestamp && <><span className="text-slate-300">•</span><span className="inline-flex items-center gap-1"><Clock3 className="h-3 w-3" />{new Intl.DateTimeFormat(undefined, { hour: "numeric", minute: "2-digit" }).format(new Date(timestamp))}</span></>}
                    </div>
                  </div>
                </Link>
              );
            })}
          </div>
        </section>
      )}
    </div>
  );
}
