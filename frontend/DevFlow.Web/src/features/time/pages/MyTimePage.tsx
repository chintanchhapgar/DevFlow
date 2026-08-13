import { useMemo, useState } from "react";
import {
  CalendarDays,
  Clock3,
  FolderKanban,
  Play,
  Timer,
} from "lucide-react";
import { Link } from "react-router-dom";

import { Button } from "@/components/ui/button";
import { useMyTime } from "@/features/projects/hooks/use-my-time";

function startOfWeek(date: Date) {
  const copy = new Date(date);
  const day = copy.getDay();
  const offset = day === 0 ? -6 : 1 - day;

  copy.setDate(copy.getDate() + offset);
  copy.setHours(0, 0, 0, 0);

  return copy;
}

function addDays(date: Date, days: number) {
  const copy = new Date(date);
  copy.setDate(copy.getDate() + days);

  return copy;
}

function dateKey(value: Date | string) {
  const date = new Date(value);

  return [
    date.getFullYear(),
    String(date.getMonth() + 1).padStart(2, "0"),
    String(date.getDate()).padStart(2, "0"),
  ].join("-");
}

function formatMinutes(minutes: number) {
  const hours = Math.floor(minutes / 60);
  const remainder = minutes % 60;

  if (!hours) {
    return `${remainder}m`;
  }

  return remainder ? `${hours}h ${remainder}m` : `${hours}h`;
}

export function MyTimePage() {
  const myTimeQuery = useMyTime();
  const [weekOffset, setWeekOffset] = useState(0);

  const weekStart = useMemo(() => {
    const currentWeek = startOfWeek(new Date());

    return addDays(currentWeek, weekOffset * 7);
  }, [weekOffset]);

  const weekDays = useMemo(
    () => Array.from({ length: 7 }, (_, index) => addDays(weekStart, index)),
    [weekStart],
  );

  const weekEnd = addDays(weekStart, 6);

  const summary = useMemo(() => {
    const today = dateKey(new Date());
    const weekStartKey = dateKey(weekStart);
    const weekEndKey = dateKey(weekEnd);

    const weekEntries = myTimeQuery.entries.filter((entry) => {
      const entryDate = dateKey(entry.startedAtUtc);

      return entryDate >= weekStartKey && entryDate <= weekEndKey;
    });

    const todayMinutes = myTimeQuery.entries
      .filter((entry) => dateKey(entry.startedAtUtc) === today)
      .reduce((total, entry) => total + entry.minutesSpent, 0);

    const weekMinutes = weekEntries.reduce(
      (total, entry) => total + entry.minutesSpent,
      0,
    );

    const runningEntry =
      myTimeQuery.entries.find((entry) => entry.isRunning) ?? null;

    const dailyMinutes = new Map<string, number>();

    weekEntries.forEach((entry) => {
      const key = dateKey(entry.startedAtUtc);

      dailyMinutes.set(
        key,
        (dailyMinutes.get(key) ?? 0) + entry.minutesSpent,
      );
    });

    return {
      weekEntries,
      todayMinutes,
      weekMinutes,
      runningEntry,
      dailyMinutes,
    };
  }, [myTimeQuery.entries, weekEnd, weekStart]);

  const weekLabel = new Intl.DateTimeFormat(undefined, {
    day: "numeric",
    month: "short",
  });

  return (
    <div className="mx-auto w-full max-w-6xl space-y-6">
      <div>
        <p className="text-sm font-medium text-[var(--devflow-primary)]">
          Workspace
        </p>

        <h1 className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
          My time
        </h1>

        <p className="mt-1.5 text-sm text-slate-500">
          Review the time you have logged across your work items.
        </p>
      </div>

      {myTimeQuery.isError && (
        <section className="rounded-2xl border border-red-200 bg-red-50 p-5">
          <p className="font-medium text-red-800">
            Unable to load your time entries.
          </p>

          <Button
            type="button"
            size="sm"
            variant="outline"
            className="mt-3"
            onClick={() => myTimeQuery.refetch()}
          >
            Try again
          </Button>
        </section>
      )}

      <section className="grid gap-4 md:grid-cols-3">
        <TimeCard
          icon={Clock3}
          label="Today"
          value={formatMinutes(summary.todayMinutes)}
          description="Time logged today"
          className="bg-sky-50 text-sky-600"
        />

        <TimeCard
          icon={CalendarDays}
          label="This week"
          value={formatMinutes(summary.weekMinutes)}
          description="Time logged in this week"
          className="bg-violet-50 text-violet-600"
        />

        <TimeCard
          icon={summary.runningEntry ? Play : Timer}
          label="Timer"
          value={summary.runningEntry ? "Running" : "Stopped"}
          description={
            summary.runningEntry
              ? `${summary.runningEntry.workItemKey} · ${summary.runningEntry.workItemTitle}`
              : "No active timer"
          }
          className={
            summary.runningEntry
              ? "bg-emerald-50 text-emerald-600"
              : "bg-slate-100 text-slate-500"
          }
        />
      </section>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <h2 className="text-sm font-semibold text-slate-900">
              Weekly overview
            </h2>

            <p className="mt-1 text-xs text-slate-500">
              {weekLabel.format(weekStart)} – {weekLabel.format(weekEnd)}
            </p>
          </div>

          <div className="flex gap-2">
            <Button
              type="button"
              size="sm"
              variant="outline"
              onClick={() => setWeekOffset((current) => current - 1)}
            >
              Previous
            </Button>

            <Button
              type="button"
              size="sm"
              variant="outline"
              disabled={weekOffset === 0}
              onClick={() => setWeekOffset(0)}
            >
              This week
            </Button>

            <Button
              type="button"
              size="sm"
              variant="outline"
              disabled={weekOffset === 0}
              onClick={() => setWeekOffset((current) => current + 1)}
            >
              Next
            </Button>
          </div>
        </div>

        <div className="grid divide-y divide-slate-100 sm:grid-cols-7 sm:divide-x sm:divide-y-0">
          {weekDays.map((day) => {
            const minutes = summary.dailyMinutes.get(dateKey(day)) ?? 0;
            const isToday = dateKey(day) === dateKey(new Date());

            return (
              <div key={dateKey(day)} className="p-4 text-center">
                <p className="text-xs font-medium text-slate-400">
                  {new Intl.DateTimeFormat(undefined, {
                    weekday: "short",
                  }).format(day)}
                </p>

                <p
                  className={`mt-1 text-sm font-semibold ${
                    isToday
                      ? "text-[var(--devflow-primary)]"
                      : "text-slate-800"
                  }`}
                >
                  {new Intl.DateTimeFormat(undefined, {
                    day: "numeric",
                  }).format(day)}
                </p>

                <p className="mt-3 text-sm font-semibold text-slate-700">
                  {formatMinutes(minutes)}
                </p>
              </div>
            );
          })}
        </div>
      </section>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <div className="border-b border-slate-100 px-5 py-4">
          <h2 className="text-sm font-semibold text-slate-900">
            Time entries
          </h2>

          <p className="mt-1 text-xs text-slate-500">
            Entries logged during the selected week.
          </p>
        </div>

        {myTimeQuery.isLoading && (
          <div className="space-y-3 p-5">
            {[0, 1, 2].map((index) => (
              <div
                key={index}
                className="h-14 animate-pulse rounded-lg bg-slate-100"
              />
            ))}
          </div>
        )}

        {!myTimeQuery.isLoading &&
          !myTimeQuery.isError &&
          summary.weekEntries.length === 0 && (
            <div className="px-5 py-14 text-center">
              <Clock3 className="mx-auto h-7 w-7 text-slate-400" />

              <p className="mt-3 text-sm font-medium text-slate-700">
                No time logged this week
              </p>

              <p className="mt-1 text-xs text-slate-400">
                Start a timer or log time from a work item.
              </p>
            </div>
          )}

        {!myTimeQuery.isLoading &&
          !myTimeQuery.isError &&
          summary.weekEntries.length > 0 && (
            <div className="divide-y divide-slate-100">
              {summary.weekEntries.map((entry) => (
                <Link
                  key={entry.worklogId}
                  to={`/projects/${entry.projectId}`}
                  className="flex items-center gap-3 px-5 py-4 transition-colors hover:bg-slate-50"
                >
                  <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl bg-slate-100 text-slate-500">
                    <FolderKanban className="h-4 w-4" />
                  </div>

                  <div className="min-w-0 flex-1">
                    <p className="truncate text-sm font-medium text-slate-800">
                      {entry.workItemTitle}
                    </p>

                    <p className="mt-1 text-xs text-slate-500">
                      <span className="font-medium text-slate-600">
                        {entry.workItemKey}
                      </span>
                      {" · "}
                      {entry.projectName}
                      {" · "}
                      {new Intl.DateTimeFormat(undefined, {
                        day: "numeric",
                        month: "short",
                        hour: "numeric",
                        minute: "2-digit",
                      }).format(new Date(entry.startedAtUtc))}
                    </p>
                  </div>

                  <span className="shrink-0 text-sm font-semibold text-slate-700">
                    {entry.isRunning
                      ? "Running"
                      : formatMinutes(entry.minutesSpent)}
                  </span>
                </Link>
              ))}
            </div>
          )}
      </section>
    </div>
  );
}

function TimeCard({
  icon: Icon,
  label,
  value,
  description,
  className,
}: {
  icon: React.ComponentType<{ className?: string }>;
  label: string;
  value: string;
  description: string;
  className: string;
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
          className={`flex h-10 w-10 items-center justify-center rounded-xl ${className}`}
        >
          <Icon className="h-5 w-5" />
        </div>
      </div>

      <p className="mt-3 truncate text-xs text-slate-400">
        {description}
      </p>
    </section>
  );
}