import { useState } from "react";
import { LoaderCircle } from "lucide-react";

import { Button } from "@/components/ui/button";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import type { Sprint } from "../api/sprints-api";
import { useCreateSprint, useDeleteSprint, useUpdateSprint } from "../hooks/use-sprints";

type Props =
  | { mode: "create"; open: boolean; onOpenChange: (open: boolean) => void; projectId: string; sprint?: never }
  | { mode: "edit"; open: boolean; onOpenChange: (open: boolean) => void; projectId: string; sprint: Sprint };

type Values = { name: string; goal: string; startDate: string; endDate: string };
const emptyValues: Values = { name: "", goal: "", startDate: "", endDate: "" };

export function SprintDialog(props: Props) {
  const createSprint = useCreateSprint();
  const updateSprint = useUpdateSprint();
  const deleteSprint = useDeleteSprint();
  const [values, setValues] = useState<Values>(() => props.mode === "edit" ? {
    name: props.sprint.name,
    goal: props.sprint.goal ?? "",
    startDate: props.sprint.startDate.slice(0, 10),
    endDate: props.sprint.endDate.slice(0, 10),
  } : emptyValues);
  const [error, setError] = useState<string | null>(null);
  const [confirmDelete, setConfirmDelete] = useState(false);
  const isSubmitting = createSprint.isPending || updateSprint.isPending || deleteSprint.isPending;

  const setValue = (key: keyof Values, value: string) => setValues((current) => ({ ...current, [key]: value }));

  async function submit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!values.name.trim() || !values.startDate || !values.endDate) {
      setError("Name, start date, and end date are required.");
      return;
    }
    if (values.endDate < values.startDate) {
      setError("The end date must be on or after the start date.");
      return;
    }
    setError(null);
    const request = { name: values.name.trim(), goal: values.goal.trim() || null, startDate: values.startDate, endDate: values.endDate };
    try {
      if (props.mode === "create") await createSprint.mutateAsync({ projectId: props.projectId, request });
      else await updateSprint.mutateAsync({ projectId: props.projectId, sprintId: props.sprint.sprintId, request });
      props.onOpenChange(false);
    } catch {
      setError("Unable to save the sprint. Please try again.");
    }
  }

  async function remove() {
    if (props.mode !== "edit") return;
    setError(null);
    try {
      await deleteSprint.mutateAsync({ projectId: props.projectId, sprintId: props.sprint.sprintId });
      props.onOpenChange(false);
    } catch {
      setError("Unable to delete this sprint. Only planned sprints can be deleted.");
    }
  }

  return <Dialog open={props.open} onOpenChange={props.onOpenChange}>
    <DialogContent className="sm:max-w-lg">
      <DialogHeader>
        <DialogTitle>{props.mode === "create" ? "Create sprint" : "Edit sprint"}</DialogTitle>
        <DialogDescription>{props.mode === "create" ? "Set the scope and schedule before starting the sprint." : "Only planned sprints can be edited or deleted."}</DialogDescription>
      </DialogHeader>
      {confirmDelete ? <div className="space-y-5 px-6 py-5">
        <div className="rounded-lg border border-red-200 bg-red-50 p-4 text-sm text-red-800">Delete this planned sprint? Work items will remain in the project backlog.</div>
        {error && <p className="text-sm text-red-600">{error}</p>}
        <DialogFooter><Button type="button" variant="outline" disabled={isSubmitting} onClick={() => setConfirmDelete(false)}>Cancel</Button><Button type="button" variant="destructive" disabled={isSubmitting} onClick={remove}>{deleteSprint.isPending && <LoaderCircle className="h-4 w-4 animate-spin" />}Delete sprint</Button></DialogFooter>
      </div> : <form className="space-y-5 px-6 py-5" onSubmit={submit}>
        <div className="space-y-2"><Label htmlFor="sprint-name">Name</Label><Input id="sprint-name" value={values.name} maxLength={200} placeholder="e.g. Sprint 12" onChange={(event) => setValue("name", event.target.value)} /></div>
        <div className="space-y-2"><Label htmlFor="sprint-goal">Goal</Label><textarea id="sprint-goal" value={values.goal} rows={3} maxLength={1000} placeholder="What should this sprint achieve?" onChange={(event) => setValue("goal", event.target.value)} className="flex w-full rounded-lg border border-slate-200 bg-white px-3 py-2 text-sm outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200" /></div>
        <div className="grid gap-4 sm:grid-cols-2"><div className="space-y-2"><Label htmlFor="sprint-start">Start date</Label><Input id="sprint-start" type="date" value={values.startDate} onChange={(event) => setValue("startDate", event.target.value)} /></div><div className="space-y-2"><Label htmlFor="sprint-end">End date</Label><Input id="sprint-end" type="date" value={values.endDate} onChange={(event) => setValue("endDate", event.target.value)} /></div></div>
        {error && <p className="text-sm text-red-600">{error}</p>}
        <DialogFooter>{props.mode === "edit" && <Button type="button" variant="destructive" className="mr-auto" disabled={isSubmitting} onClick={() => setConfirmDelete(true)}>Delete</Button>}<Button type="button" variant="outline" disabled={isSubmitting} onClick={() => props.onOpenChange(false)}>Cancel</Button><Button type="submit" disabled={isSubmitting}>{isSubmitting && <LoaderCircle className="h-4 w-4 animate-spin" />}{props.mode === "create" ? "Create sprint" : "Save changes"}</Button></DialogFooter>
      </form>}
    </DialogContent>
  </Dialog>;
}
