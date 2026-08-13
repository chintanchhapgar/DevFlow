import { useEffect, useMemo, useState } from "react";
import {
  Clock3,
  LoaderCircle,
  Pause,
  Play,
  Plus,
  X,
} from "lucide-react";
import { Button } from "@/components/ui/button";

import {
  useCreateWorklog,
  useStartTimer,
  useStopTimer,
  useWorklogs,
} from "../hooks/use-worklogs";

function formatDuration(totalSeconds: number) {
  const hours = Math.floor(totalSeconds / 3_600);
  const minutes = Math.floor((totalSeconds % 3_600) / 60);
  const seconds = totalSeconds % 60;

  if (hours > 0) {
    return `${hours}h ${minutes}m ${seconds}s`;
  }

  return `${minutes}m ${seconds}s`;
}

function formatMinutes(minutes: number) {
  if (minutes < 60) {
    return `${minutes}m`;
  }

  const hours = Math.floor(minutes / 60);
  const remainingMinutes = minutes % 60;

  return remainingMinutes
    ? `${hours}h ${remainingMinutes}m`
    : `${hours}h`;
}

export function WorklogPanel({
  workItemId,
}: {
  workItemId: string;
}) {
  const worklogsQuery = useWorklogs(workItemId);
  const startTimer = useStartTimer();
  const stopTimer = useStopTimer();

  const createWorklog = useCreateWorklog();

    const [isManualEntryOpen, setIsManualEntryOpen] =
    useState(false);
    const [description, setDescription] = useState("");
    const [entryDate, setEntryDate] = useState(
    new Date().toISOString().slice(0, 10),
    );
    const [hours, setHours] = useState("");
    const [minutes, setMinutes] = useState("");

  const [now, setNow] = useState(() => Date.now());
  const [error, setError] = useState<string | null>(null);

  const runningWorklog = useMemo(
    () =>
      worklogsQuery.data?.find((worklog) => worklog.isRunning) ??
      null,
    [worklogsQuery.data],
  );

  useEffect(() => {
    if (!runningWorklog) {
      return;
    }

    const interval = window.setInterval(() => {
      setNow(Date.now());
    }, 1_000);

    return () => window.clearInterval(interval);
  }, [runningWorklog]);

  const elapsedSeconds = runningWorklog
    ? Math.max(
        0,
        Math.floor(
          (now -
            new Date(runningWorklog.startedAtUtc).getTime()) /
            1_000,
        ),
      )
    : 0;

  const totalMinutes =
    worklogsQuery.data?.reduce(
      (total, worklog) => total + worklog.minutesSpent,
      0,
    ) ?? 0;

  async function handleTimer() {
    setError(null);

    try {
      if (runningWorklog) {
        await stopTimer.mutateAsync({ workItemId });
      } else {
        await startTimer.mutateAsync({ workItemId });
      }
    } catch {
      setError(
        runningWorklog
          ? "Unable to stop the timer. Please try again."
          : "Unable to start the timer. Please try again.",
      );
    }
  }

  const isSaving =
  startTimer.isPending ||
  stopTimer.isPending ||
  createWorklog.isPending;

  async function handleManualEntry(
  event: React.FormEvent<HTMLFormElement>,
) {
  event.preventDefault();
  setError(null);

  const parsedHours = Number(hours || 0);
  const parsedMinutes = Number(minutes || 0);
  const totalMinutes = parsedHours * 60 + parsedMinutes;

  if (
    !Number.isFinite(totalMinutes) ||
    totalMinutes <= 0 ||
    parsedMinutes >= 60 ||
    parsedHours < 0 ||
    parsedMinutes < 0
  ) {
    setError("Enter a valid duration.");
    return;
  }

  const endedAt = new Date(`${entryDate}T12:00:00`);
  const startedAt = new Date(
    endedAt.getTime() - totalMinutes * 60_000,
  );

  try {
    await createWorklog.mutateAsync({
      workItemId,
      description: description.trim() || null,
      startedAtUtc: startedAt.toISOString(),
      endedAtUtc: endedAt.toISOString(),
    });

    setDescription("");
    setHours("");
    setMinutes("");
    setIsManualEntryOpen(false);
  } catch {
    setError("Unable to log time. Please try again.");
  }
}

  return (
    <section className="rounded-xl border border-slate-200 bg-white p-4">
      <div className="flex items-start justify-between gap-4">
        <div>
          <h3 className="text-sm font-semibold text-slate-900">
            Time tracking
          </h3>

          <p className="mt-1 text-xs text-slate-500">
            {totalMinutes > 0
              ? `${formatMinutes(totalMinutes)} logged on this work item`
              : "No time logged yet"}
          </p>
        </div>

       <div className="flex shrink-0 gap-2">
        <Button
            type="button"
            size="sm"
            variant="outline"
            disabled={isSaving}
            onClick={() => {
            setError(null);
            setIsManualEntryOpen((current) => !current);
            }}
        >
            {isManualEntryOpen ? (
            <X className="h-4 w-4" />
            ) : (
            <Plus className="h-4 w-4" />
            )}

            Log time
        </Button>

        <Button
            type="button"
            size="sm"
            disabled={isSaving}
            onClick={handleTimer}
            variant={runningWorklog ? "outline" : "default"}
        >
            {isSaving ? (
            <LoaderCircle className="h-4 w-4 animate-spin" />
            ) : runningWorklog ? (
            <Pause className="h-4 w-4" />
            ) : (
            <Play className="h-4 w-4" />
            )}

            {runningWorklog ? "Stop timer" : "Start timer"}
        </Button>
        </div>
      </div>

      {isManualEntryOpen && (
        <form
            onSubmit={handleManualEntry}
            className="mt-4 rounded-lg border border-slate-200 bg-slate-50 p-3"
        >
            <div className="grid gap-3 sm:grid-cols-2">
            <label className="text-xs font-medium text-slate-600">
                Date
                <input
                type="date"
                required
                value={entryDate}
                max={new Date().toISOString().slice(0, 10)}
                onChange={(event) => setEntryDate(event.target.value)}
                className="mt-1.5 h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                />
            </label>

            <label className="text-xs font-medium text-slate-600">
                Description
                <input
                type="text"
                value={description}
                maxLength={500}
                placeholder="What did you work on?"
                onChange={(event) => setDescription(event.target.value)}
                className="mt-1.5 h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                />
            </label>

            <label className="text-xs font-medium text-slate-600">
                Hours
                <input
                type="number"
                min="0"
                max="23"
                value={hours}
                placeholder="0"
                onChange={(event) => setHours(event.target.value)}
                className="mt-1.5 h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                />
            </label>

            <label className="text-xs font-medium text-slate-600">
                Minutes
                <input
                type="number"
                min="0"
                max="59"
                value={minutes}
                placeholder="0"
                onChange={(event) => setMinutes(event.target.value)}
                className="mt-1.5 h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                />
            </label>
            </div>

            <div className="mt-3 flex justify-end gap-2">
            <Button
                type="button"
                size="sm"
                variant="outline"
                onClick={() => setIsManualEntryOpen(false)}
            >
                Cancel
            </Button>

            <Button type="submit" size="sm" disabled={isSaving}>
                {createWorklog.isPending && (
                <LoaderCircle className="h-4 w-4 animate-spin" />
                )}
                Save entry
            </Button>
            </div>
        </form>
        )}

      {runningWorklog && (
        <div className="mt-4 flex items-center gap-3 rounded-lg border border-emerald-100 bg-emerald-50 px-3 py-2.5">
          <span className="flex h-2 w-2 shrink-0 rounded-full bg-emerald-500" />

          <div className="min-w-0 flex-1">
            <p className="text-xs font-medium text-emerald-800">
              Timer running
            </p>

            <p className="mt-0.5 text-lg font-semibold tabular-nums text-emerald-900">
              {formatDuration(elapsedSeconds)}
            </p>
          </div>
        </div>
      )}

      {error && (
        <p className="mt-3 rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-xs text-red-700">
          {error}
        </p>
      )}

      {worklogsQuery.isLoading && (
        <div className="mt-4 h-16 animate-pulse rounded-lg bg-slate-100" />
      )}

      {!worklogsQuery.isLoading &&
        !worklogsQuery.isError &&
        worklogsQuery.data &&
        worklogsQuery.data.length > 0 && (
          <div className="mt-4 border-t border-slate-100 pt-3">
            <p className="text-xs font-semibold uppercase tracking-wider text-slate-400">
              Recent entries
            </p>

            <div className="mt-2 space-y-2">
              {worklogsQuery.data.slice(0, 4).map((worklog) => (
                <div
                  key={worklog.worklogId}
                  className="flex items-center gap-3 text-sm"
                >
                  <div className="flex h-7 w-7 shrink-0 items-center justify-center rounded-lg bg-slate-100 text-slate-500">
                    <Clock3 className="h-3.5 w-3.5" />
                  </div>

                  <div className="min-w-0 flex-1">
                    <p className="truncate text-xs font-medium text-slate-700">
                      {worklog.description || "Time entry"}
                    </p>

                    <p className="mt-0.5 text-[11px] text-slate-400">
                      {new Intl.DateTimeFormat(undefined, {
                        day: "numeric",
                        month: "short",
                        hour: "numeric",
                        minute: "2-digit",
                      }).format(new Date(worklog.startedAtUtc))}
                    </p>
                  </div>

                  <span className="shrink-0 text-xs font-semibold text-slate-600">
                    {worklog.isRunning
                      ? "Running"
                      : formatMinutes(worklog.minutesSpent)}
                  </span>
                </div>
              ))}
            </div>
          </div>
        )}

      {worklogsQuery.isError && (
        <p className="mt-3 text-xs text-red-600">
          Unable to load time entries.
        </p>
      )}
    </section>
    
  );
}