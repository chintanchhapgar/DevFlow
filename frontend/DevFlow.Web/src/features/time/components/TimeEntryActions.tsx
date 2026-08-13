import { useState } from "react";
import {
  LoaderCircle,
  Pencil,
  Trash2,
} from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import {
  deleteWorklog,
  updateWorklog,
} from "@/features/projects/api/worklogs-api";
import type { MyTimeEntry } from "@/features/projects/hooks/use-my-time";

function toDateInput(value: string) {
  return new Date(value).toISOString().slice(0, 10);
}

function getDuration(entry: MyTimeEntry) {
  return {
    hours: Math.floor(entry.minutesSpent / 60).toString(),
    minutes: (entry.minutesSpent % 60).toString(),
  };
}

const fieldClassName =
  "mt-2 h-11 w-full rounded-lg border border-slate-300 bg-white px-3 text-sm text-slate-800 shadow-sm outline-none placeholder:text-slate-400 focus:border-[var(--devflow-primary)] focus:ring-2 focus:ring-[var(--devflow-primary)]/20";

export function TimeEntryActions({
  entry,
  onChanged,
}: {
  entry: MyTimeEntry;
  onChanged: () => Promise<unknown>;
}) {
  const initialDuration = getDuration(entry);

  const [isEditOpen, setIsEditOpen] = useState(false);
  const [description, setDescription] = useState(
    entry.description ?? "",
  );
  const [entryDate, setEntryDate] = useState(
    toDateInput(entry.startedAtUtc),
  );
  const [hours, setHours] = useState(initialDuration.hours);
  const [minutes, setMinutes] = useState(
    initialDuration.minutes,
  );
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleSave(
    event: React.FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    const parsedHours = Number(hours || 0);
    const parsedMinutes = Number(minutes || 0);
    const totalMinutes = parsedHours * 60 + parsedMinutes;

    if (
      !Number.isFinite(totalMinutes) ||
      totalMinutes <= 0 ||
      parsedHours < 0 ||
      parsedMinutes < 0 ||
      parsedMinutes >= 60
    ) {
      setError("Enter a valid duration.");
      return;
    }

    setError(null);
    setIsSaving(true);

    try {
      const endedAt = new Date(`${entryDate}T12:00:00`);
      const startedAt = new Date(
        endedAt.getTime() - totalMinutes * 60_000,
      );

      await updateWorklog(entry.worklogId, {
        description: description.trim() || null,
        startedAtUtc: startedAt.toISOString(),
        endedAtUtc: endedAt.toISOString(),
      });

      await onChanged();
      setIsEditOpen(false);
    } catch {
      setError("Unable to update this time entry.");
    } finally {
      setIsSaving(false);
    }
  }

  async function handleDelete() {
    const confirmed = window.confirm(
      "Delete this time entry? This cannot be undone.",
    );

    if (!confirmed) {
      return;
    }

    setError(null);
    setIsSaving(true);

    try {
      await deleteWorklog(entry.worklogId);
      await onChanged();
    } catch {
      setError("Unable to delete this time entry.");
    } finally {
      setIsSaving(false);
    }
  }

  if (entry.isRunning) {
    return null;
  }

  return (
    <>
      <div className="flex shrink-0 items-center gap-1">
        <Button
          type="button"
          variant="ghost"
          size="sm"
          aria-label="Edit time entry"
          title="Edit time entry"
          disabled={isSaving}
          className="h-8 w-8 p-0 text-slate-500 hover:bg-slate-100 hover:text-slate-800"
          onClick={() => {
            setError(null);
            setIsEditOpen(true);
          }}
        >
          <Pencil className="h-4 w-4" />
        </Button>

        <Button
          type="button"
          variant="ghost"
          size="sm"
          aria-label="Delete time entry"
          title="Delete time entry"
          disabled={isSaving}
          className="h-8 w-8 p-0 text-slate-400 hover:bg-red-50 hover:text-red-600"
          onClick={handleDelete}
        >
          {isSaving ? (
            <LoaderCircle className="h-4 w-4 animate-spin" />
          ) : (
            <Trash2 className="h-4 w-4" />
          )}
        </Button>
      </div>

      <Dialog open={isEditOpen} onOpenChange={setIsEditOpen}>
        <DialogContent className="sm:max-w-md">
          <DialogHeader>
            <DialogTitle>Edit time entry</DialogTitle>

            <DialogDescription>
              Update the date, duration, or a short note for this entry.
            </DialogDescription>
          </DialogHeader>

          <form onSubmit={handleSave} className="space-y-5 px-6 py-5">
            <label className="block">
              <span className="text-sm font-medium text-slate-700">
                Date
              </span>

              <input
                type="date"
                required
                max={new Date().toISOString().slice(0, 10)}
                value={entryDate}
                onChange={(event) =>
                  setEntryDate(event.target.value)
                }
                className={fieldClassName}
              />
            </label>

            <label className="block">
              <span className="text-sm font-medium text-slate-700">
                Description
              </span>

              <input
                type="text"
                maxLength={500}
                value={description}
                placeholder="What did you work on?"
                onChange={(event) =>
                  setDescription(event.target.value)
                }
                className={fieldClassName}
              />
            </label>

            <div className="grid grid-cols-2 gap-4">
              <label className="block">
                <span className="text-sm font-medium text-slate-700">
                  Hours
                </span>

                <input
                  type="number"
                  min="0"
                  max="23"
                  value={hours}
                  placeholder="0"
                  onChange={(event) => setHours(event.target.value)}
                  className={fieldClassName}
                />
              </label>

              <label className="block">
                <span className="text-sm font-medium text-slate-700">
                  Minutes
                </span>

                <input
                  type="number"
                  min="0"
                  max="59"
                  value={minutes}
                  placeholder="0"
                  onChange={(event) =>
                    setMinutes(event.target.value)
                  }
                  className={fieldClassName}
                />
              </label>
            </div>

            {error && (
              <p className="rounded-lg border border-red-200 bg-red-50 px-3 py-2.5 text-sm text-red-700">
                {error}
              </p>
            )}

            <DialogFooter className="border-t border-slate-100 pt-4">
              <Button
                type="button"
                variant="outline"
                disabled={isSaving}
                onClick={() => setIsEditOpen(false)}
              >
                Cancel
              </Button>

              <Button type="submit" disabled={isSaving}>
                {isSaving && (
                  <LoaderCircle className="h-4 w-4 animate-spin" />
                )}
                Save changes
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </>
  );
}