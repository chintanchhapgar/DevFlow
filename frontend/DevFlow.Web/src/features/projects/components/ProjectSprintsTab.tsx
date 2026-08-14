import { useState } from "react";
import { CalendarDays, LoaderCircle, Pencil, Play, Plus, CheckCircle2 } from "lucide-react";

import { Button } from "@/components/ui/button";
import type { Sprint } from "../api/sprints-api";
import { useCompleteSprint, useProjectSprints, useStartSprint } from "../hooks/use-sprints";
import { SprintDialog } from "./SprintDialog";

export function ProjectSprintsTab({ projectId }: { projectId: string }) {
  const sprintsQuery = useProjectSprints(projectId);
  const startSprint = useStartSprint();
  const completeSprint = useCompleteSprint();
  const [createOpen, setCreateOpen] = useState(false);
  const [editing, setEditing] = useState<Sprint | null>(null);
  const [error, setError] = useState<string | null>(null);
  const isTransitioning = startSprint.isPending || completeSprint.isPending;

  async function transition(sprint: Sprint, action: "start" | "complete") {
    setError(null);
    try {
      if (action === "start") await startSprint.mutateAsync({ projectId, sprintId: sprint.sprintId });
      else await completeSprint.mutateAsync({ projectId, sprintId: sprint.sprintId });
    } catch {
      setError(`Unable to ${action} this sprint. Please try again.`);
    }
  }

  if (sprintsQuery.isLoading) return <div className="space-y-3 rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">{[1, 2].map((item) => <div key={item} className="h-28 animate-pulse rounded-lg bg-slate-100" />)}</div>;
  if (sprintsQuery.isError) return <section className="rounded-2xl border border-red-200 bg-red-50 p-5"><p className="font-medium text-red-800">Unable to load sprints.</p><Button variant="outline" size="sm" className="mt-3" onClick={() => sprintsQuery.refetch()}>Try again</Button></section>;

  const sprints = sprintsQuery.data?.items ?? [];
  return <>
    <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
      <div className="flex items-center justify-between border-b border-slate-100 px-5 py-4"><div><h2 className="font-semibold text-slate-900">Sprints</h2><p className="mt-1 text-sm text-slate-500">Plan, run, and complete time-boxed delivery cycles.</p></div><Button size="sm" onClick={() => setCreateOpen(true)}><Plus className="h-4 w-4" />New sprint</Button></div>
      {error && <p className="mx-5 mt-4 rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">{error}</p>}
      {sprints.length === 0 ? <div className="flex min-h-64 flex-col items-center justify-center px-5 text-center"><CalendarDays className="h-7 w-7 text-slate-400" /><h3 className="mt-3 text-sm font-semibold text-slate-900">No sprints yet</h3><p className="mt-1 text-sm text-slate-500">Create a sprint, then assign work items to it.</p><Button variant="outline" size="sm" className="mt-4" onClick={() => setCreateOpen(true)}>Create sprint</Button></div> : <div className="divide-y divide-slate-100">{sprints.map((sprint) => <SprintRow key={sprint.sprintId} sprint={sprint} disabled={isTransitioning} onEdit={() => setEditing(sprint)} onStart={() => transition(sprint, "start")} onComplete={() => transition(sprint, "complete")} />)}</div>}
    </section>
    {createOpen && <SprintDialog mode="create" open onOpenChange={setCreateOpen} projectId={projectId} />}
    {editing && <SprintDialog mode="edit" open onOpenChange={(open) => { if (!open) setEditing(null); }} projectId={projectId} sprint={editing} />}
  </>;
}

function SprintRow({ sprint, disabled, onEdit, onStart, onComplete }: { sprint: Sprint; disabled: boolean; onEdit: () => void; onStart: () => void; onComplete: () => void }) {
  const status = typeof sprint.status === "number" ? ["Planned", "Active", "Completed", "Cancelled"][sprint.status] ?? "Unknown" : sprint.status;
  const canEdit = status === "Planned";
  return <div className="flex flex-col gap-4 px-5 py-4 sm:flex-row sm:items-center"><div className="min-w-0 flex-1"><div className="flex flex-wrap items-center gap-2"><p className="font-medium text-slate-900">{sprint.name}</p><span className={`rounded-full px-2 py-0.5 text-xs font-medium ${status === "Active" ? "bg-emerald-100 text-emerald-700" : status === "Completed" ? "bg-slate-100 text-slate-600" : "bg-blue-50 text-blue-700"}`}>{status}</span></div>{sprint.goal && <p className="mt-1 truncate text-sm text-slate-500">{sprint.goal}</p>}<p className="mt-2 text-xs text-slate-400">{formatDate(sprint.startDate)} – {formatDate(sprint.endDate)}</p></div><div className="flex shrink-0 items-center gap-2">{canEdit && <Button variant="outline" size="sm" disabled={disabled} onClick={onEdit}><Pencil className="h-4 w-4" />Edit</Button>}{canEdit && <Button size="sm" disabled={disabled} onClick={onStart}>{disabled && <LoaderCircle className="h-4 w-4 animate-spin" />}<Play className="h-4 w-4" />Start</Button>}{status === "Active" && <Button size="sm" disabled={disabled} onClick={onComplete}>{disabled && <LoaderCircle className="h-4 w-4 animate-spin" />}<CheckCircle2 className="h-4 w-4" />Complete</Button>}</div></div>;
}

function formatDate(value: string) { return new Intl.DateTimeFormat(undefined, { month: "short", day: "numeric", year: "numeric" }).format(new Date(`${value.slice(0, 10)}T00:00:00`)); }
