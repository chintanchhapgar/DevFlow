import { useState } from "react";
import {
  CalendarDays,
  CheckCircle2,
  CircleDot,
  Clock3,
  FileText,
  LoaderCircle,
  Pencil,
  UserRound,
} from "lucide-react";
import { WorklogPanel } from "./WorklogPanel";
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
  WorkItemPriority,
  WorkItemStatus,
  type WorkItem,
} from "../api/project-resources-api";
import {
  useAssignWorkItem,
  useChangeWorkItemPriority,
  useChangeWorkItemStatus,
} from "../hooks/use-project-resources";
import { AttachmentsPanel } from "./AttachmentsPanel";
import { WorkItemDialog } from "./WorkItemDialog";

type Member = {
  userId: string;
  memberName: string;
  role: string;
};

type WorkItemDetailDialogProps = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  projectId: string;
  workItem: WorkItem;
  members: Member[];
};

const statusOptions = [
  { value: WorkItemStatus.Todo, label: "To do" },
  { value: WorkItemStatus.InProgress, label: "In progress" },
  { value: WorkItemStatus.InReview, label: "In review" },
  { value: WorkItemStatus.Testing, label: "Testing" },
  { value: WorkItemStatus.Done, label: "Done" },
  { value: WorkItemStatus.Cancelled, label: "Cancelled" },
];

const priorityOptions = [
  { value: WorkItemPriority.Lowest, label: "Lowest" },
  { value: WorkItemPriority.Low, label: "Low" },
  { value: WorkItemPriority.Medium, label: "Medium" },
  { value: WorkItemPriority.High, label: "High" },
  { value: WorkItemPriority.Highest, label: "Highest" },
];

function numericValue(
  value: string | number,
  fallback: number,
) {
  return typeof value === "number" ? value : fallback;
}

function labelFor(
  value: string | number,
  options: { value: number; label: string }[],
  fallback: string,
) {
  if (typeof value === "string") {
    return value.replace(/([a-z])([A-Z])/g, "$1 $2");
  }

  return (
    options.find((option) => option.value === value)?.label ??
    fallback
  );
}

function initials(name: string) {
  return (
    name
      .trim()
      .split(/\s+/)
      .filter(Boolean)
      .slice(0, 2)
      .map((part) => part[0])
      .join("")
      .toUpperCase() || "U"
  );
}

export function WorkItemDetailDialog({
  open,
  onOpenChange,
  projectId,
  workItem,
  members,
}: WorkItemDetailDialogProps) {
  const changeStatus = useChangeWorkItemStatus();
  const changePriority = useChangeWorkItemPriority();
  const assignWorkItem = useAssignWorkItem();

  const [isEditOpen, setIsEditOpen] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const isSaving =
    changeStatus.isPending ||
    changePriority.isPending ||
    assignWorkItem.isPending;

  const statusValue = numericValue(
    workItem.status,
    WorkItemStatus.Todo,
  );
  const priorityValue = numericValue(
    workItem.priority,
    WorkItemPriority.Medium,
  );

  const assignee =
    members.find(
      (member) => member.userId === workItem.assigneeId,
    ) ?? null;

  async function handleStatusChange(value: string) {
    setError(null);

    try {
      await changeStatus.mutateAsync({
        projectId,
        workItemId: workItem.id,
        status: Number(value) as WorkItemStatus,
      });
    } catch {
      setError("Unable to update the work item status.");
    }
  }

  async function handlePriorityChange(value: string) {
    setError(null);

    try {
      await changePriority.mutateAsync({
        projectId,
        workItemId: workItem.id,
        priority: Number(value) as WorkItemPriority,
      });
    } catch {
      setError("Unable to update the work item priority.");
    }
  }

  async function handleAssigneeChange(value: string) {
    if (!value) {
      return;
    }

    setError(null);

    try {
      await assignWorkItem.mutateAsync({
        projectId,
        workItemId: workItem.id,
        assigneeId: value,
      });
    } catch {
      setError("Unable to assign this work item.");
    }
  }

  return (
    <>
      <Dialog open={open} onOpenChange={onOpenChange}>
        <DialogContent className="max-h-[92vh] overflow-y-auto sm:max-w-4xl">
          <DialogHeader>
            <div className="flex items-start gap-3 pr-10">
              <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-blue-50 text-blue-600">
                <FileText className="h-5 w-5" />
              </div>

              <div className="min-w-0">
                <p className="text-xs font-semibold uppercase tracking-wider text-slate-400">
                  {workItem.key}
                </p>

                <DialogTitle className="mt-1">
                  {workItem.title}
                </DialogTitle>

                <DialogDescription>
                  Manage workflow, ownership, and files.
                </DialogDescription>
              </div>
            </div>
          </DialogHeader>

          <div className="grid gap-6 px-6 py-5 lg:grid-cols-[minmax(0,1fr)_260px]">
            <div className="space-y-6">
              <section>
                <h3 className="text-sm font-semibold text-slate-900">
                  Description
                </h3>

                <p className="mt-2 whitespace-pre-wrap text-sm leading-6 text-slate-600">
                  {workItem.description ||
                    "No description has been added to this work item."}
                </p>
              </section>

              <AttachmentsPanel
                workItemId={workItem.id}
                workItemTitle={workItem.title}
              />

              <WorklogPanel 
                workItemId={workItem.id} 
              />
            </div>

            <aside className="space-y-4">
              <section className="rounded-xl border border-slate-200 bg-slate-50/70 p-4">
                <h3 className="text-xs font-semibold uppercase tracking-wider text-slate-400">
                  Properties
                </h3>

                <div className="mt-4 space-y-4">
                  <Property
                    icon={<CircleDot className="h-4 w-4" />}
                    label="Status"
                  >
                    <select
                      value={statusValue}
                      disabled={isSaving}
                      onChange={(event) =>
                        handleStatusChange(event.target.value)
                      }
                      className="h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm font-medium text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                    >
                      {statusOptions.map((option) => (
                        <option
                          key={option.value}
                          value={option.value}
                        >
                          {option.label}
                        </option>
                      ))}
                    </select>
                  </Property>

                  <Property
                    icon={<CheckCircle2 className="h-4 w-4" />}
                    label="Priority"
                  >
                    <select
                      value={priorityValue}
                      disabled={isSaving}
                      onChange={(event) =>
                        handlePriorityChange(event.target.value)
                      }
                      className="h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm font-medium text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                    >
                      {priorityOptions.map((option) => (
                        <option
                          key={option.value}
                          value={option.value}
                        >
                          {option.label}
                        </option>
                      ))}
                    </select>
                  </Property>

                  <Property
                    icon={<UserRound className="h-4 w-4" />}
                    label="Assignee"
                  >
                    <select
                      value={workItem.assigneeId ?? ""}
                      disabled={isSaving || members.length === 0}
                      onChange={(event) =>
                        handleAssigneeChange(event.target.value)
                      }
                      className="h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm font-medium text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                    >
                      <option value="">
                        {assignee
                          ? assignee.memberName
                          : "Unassigned"}
                      </option>

                      {members.map((member) => (
                        <option
                          key={member.userId}
                          value={member.userId}
                        >
                          {member.memberName || member.userId}
                        </option>
                      ))}
                    </select>
                  </Property>

                  <Property
                    icon={<Clock3 className="h-4 w-4" />}
                    label="Estimate"
                  >
                    <p className="text-sm font-medium text-slate-700">
                      {workItem.estimateHours
                        ? `${workItem.estimateHours} hours`
                        : "Not estimated"}
                    </p>
                  </Property>

                  <Property
                    icon={<CalendarDays className="h-4 w-4" />}
                    label="Due date"
                  >
                    <p className="text-sm font-medium text-slate-700">
                      {workItem.dueDate
                        ? new Intl.DateTimeFormat(undefined, {
                            dateStyle: "medium",
                          }).format(new Date(workItem.dueDate))
                        : "No due date"}
                    </p>
                  </Property>
                </div>
              </section>

              <section className="rounded-xl border border-slate-200 p-4">
                <p className="text-xs font-semibold uppercase tracking-wider text-slate-400">
                  Summary
                </p>

                <dl className="mt-3 space-y-2 text-sm">
                  <div className="flex justify-between gap-3">
                    <dt className="text-slate-500">Type</dt>
                    <dd className="font-medium text-slate-700">
                      {labelFor(
                        workItem.type,
                        [],
                        "Work item",
                      )}
                    </dd>
                  </div>

                  <div className="flex justify-between gap-3">
                    <dt className="text-slate-500">Status</dt>
                    <dd className="font-medium text-slate-700">
                      {labelFor(
                        workItem.status,
                        statusOptions,
                        "To do",
                      )}
                    </dd>
                  </div>

                  <div className="flex justify-between gap-3">
                    <dt className="text-slate-500">Priority</dt>
                    <dd className="font-medium text-slate-700">
                      {labelFor(
                        workItem.priority,
                        priorityOptions,
                        "Medium",
                      )}
                    </dd>
                  </div>

                  {assignee && (
                    <div className="flex items-center justify-between gap-3">
                      <dt className="text-slate-500">Assigned to</dt>
                      <dd className="flex items-center gap-1.5 font-medium text-slate-700">
                        <span className="flex h-5 w-5 items-center justify-center rounded-full bg-slate-100 text-[9px]">
                          {initials(assignee.memberName)}
                        </span>
                        <span className="max-w-28 truncate">
                          {assignee.memberName}
                        </span>
                      </dd>
                    </div>
                  )}
                </dl>
              </section>

              {error && (
                <p className="rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
                  {error}
                </p>
              )}

              {isSaving && (
                <p className="flex items-center gap-2 text-xs text-slate-500">
                  <LoaderCircle className="h-3.5 w-3.5 animate-spin" />
                  Saving changes…
                </p>
              )}
            </aside>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={() => onOpenChange(false)}
            >
              Close
            </Button>

            <Button
              type="button"
              onClick={() => setIsEditOpen(true)}
            >
              <Pencil className="h-4 w-4" />
              Edit work item
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      <WorkItemDialog
        mode="edit"
        open={isEditOpen}
        onOpenChange={setIsEditOpen}
        projectId={projectId}
        workItem={workItem}
      />
    </>
  );
}

function Property({
  icon,
  label,
  children,
}: {
  icon: React.ReactNode;
  label: string;
  children: React.ReactNode;
}) {
  return (
    <div>
      <p className="flex items-center gap-2 text-xs font-medium text-slate-500">
        <span className="text-slate-400">{icon}</span>
        {label}
      </p>

      <div className="mt-1.5">{children}</div>
    </div>
  );
}