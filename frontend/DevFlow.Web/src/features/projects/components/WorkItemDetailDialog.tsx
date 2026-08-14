import {
  useEffect,
  useState,
} from "react";
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
  useMoveWorkItemToSprint,
} from "../hooks/use-project-resources";
import { useProjectSprints } from "../hooks/use-sprints";
import { AttachmentsPanel } from "./AttachmentsPanel";
import { WorkItemDialog } from "./WorkItemDialog";
import { WorklogPanel } from "./WorklogPanel";

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

function getStatusValue(value: string | number) {
  if (typeof value === "number") {
    return value as WorkItemStatus;
  }

  const values: Record<string, WorkItemStatus> = {
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

  return values[value.toLowerCase()] ?? WorkItemStatus.Todo;
}

function getPriorityValue(value: string | number) {
  if (typeof value === "number") {
    return value as WorkItemPriority;
  }

  const values: Record<string, WorkItemPriority> = {
    lowest: WorkItemPriority.Lowest,
    low: WorkItemPriority.Low,
    medium: WorkItemPriority.Medium,
    high: WorkItemPriority.High,
    highest: WorkItemPriority.Highest,
  };

  return values[value.toLowerCase()] ?? WorkItemPriority.Medium;
}

function labelFor(
  value: string | number,
  options: { value: number; label: string }[],
  fallback: string,
) {
  const numericValue =
    options === statusOptions
      ? getStatusValue(value)
      : getPriorityValue(value);

  return (
    options.find((option) => option.value === numericValue)?.label ??
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
  const moveWorkItemToSprint = useMoveWorkItemToSprint();
  const sprintsQuery = useProjectSprints(projectId);

  const [isEditOpen, setIsEditOpen] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [selectedStatus, setSelectedStatus] = useState(
    getStatusValue(workItem.status),
  );

  const [selectedPriority, setSelectedPriority] = useState(
    getPriorityValue(workItem.priority),
  );

  const [selectedAssigneeId, setSelectedAssigneeId] = useState(
    workItem.assigneeId ?? "",
  );

  const [selectedSprintId, setSelectedSprintId] = useState(
    workItem.sprintId ?? "",
  );





  useEffect(() => {
    setSelectedStatus(getStatusValue(workItem.status));
    setSelectedPriority(getPriorityValue(workItem.priority));
    setSelectedAssigneeId(workItem.assigneeId ?? "");
  }, [
    workItem.id,
    workItem.status,
    workItem.priority,
    workItem.assigneeId,
  ]);


    useEffect(() => {
    if (!sprintsQuery.data) return; // Wait until sprints are loaded

    setSelectedSprintId(workItem.sprintId ?? "");
  }, [workItem.sprintId, sprintsQuery.data]);

  const isSaving =
    changeStatus.isPending ||
    changePriority.isPending ||
    assignWorkItem.isPending ||
    moveWorkItemToSprint.isPending;

  const assignee =
    members.find(
      (member) => member.userId === selectedAssigneeId,
    ) ?? null;

  async function handleStatusChange(value: string) {
    const nextStatus = Number(value) as WorkItemStatus;
    const previousStatus = selectedStatus;

    setError(null);
    setSelectedStatus(nextStatus);

    try {
      await changeStatus.mutateAsync({
        projectId,
        workItemId: workItem.id,
        status: nextStatus,
      });
    } catch {
      setSelectedStatus(previousStatus);
      setError("Unable to update the work item status.");
    }
  }

  async function handlePriorityChange(value: string) {
    const nextPriority = Number(value) as WorkItemPriority;
    const previousPriority = selectedPriority;

    setError(null);
    setSelectedPriority(nextPriority);

    try {
      await changePriority.mutateAsync({
        projectId,
        workItemId: workItem.id,
        priority: nextPriority,
      });
    } catch {
      setSelectedPriority(previousPriority);
      setError("Unable to update the work item priority.");
    }
  }

  async function handleAssigneeChange(value: string) {
    if (!value) {
      return;
    }

    const previousAssigneeId = selectedAssigneeId;

    setError(null);
    setSelectedAssigneeId(value);

    try {
      await assignWorkItem.mutateAsync({
        projectId,
        workItemId: workItem.id,
        assigneeId: value,
      });
    } catch {
      setSelectedAssigneeId(previousAssigneeId);
      setError("Unable to assign this work item.");
    }
  }

  async function handleSprintChange(value: string) {
    if (!value) {
      return;
    }

    const previousSprintId = selectedSprintId;

    setError(null);
    setSelectedSprintId(value);

    try {
      await moveWorkItemToSprint.mutateAsync({
        projectId,
        workItemId: workItem.id,
        sprintId: value,
      });
    } catch {
      setSelectedSprintId(previousSprintId);
      setError("Unable to move this work item to the sprint.");
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
                  Manage workflow, ownership, time, and files.
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

              <WorklogPanel workItemId={workItem.id} />
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
                      value={selectedStatus}
                      disabled={isSaving}
                      onChange={(event) =>
                        handleStatusChange(event.target.value)
                      }
                      className="h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm font-medium text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200 disabled:cursor-not-allowed disabled:bg-slate-100"
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
                      value={selectedPriority}
                      disabled={isSaving}
                      onChange={(event) =>
                        handlePriorityChange(event.target.value)
                      }
                      className="h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm font-medium text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200 disabled:cursor-not-allowed disabled:bg-slate-100"
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
                      value={selectedAssigneeId}
                      disabled={isSaving || members.length === 0}
                      onChange={(event) =>
                        handleAssigneeChange(event.target.value)
                      }
                      className="h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm font-medium text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200 disabled:cursor-not-allowed disabled:bg-slate-100"
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
                    icon={<CalendarDays className="h-4 w-4" />}
                    label="Sprint"
                  >
                    <select
                      value={selectedSprintId}
                      disabled={
                        isSaving ||
                        sprintsQuery.isLoading ||
                        sprintsQuery.isError
                      }
                      onChange={(event) =>
                        handleSprintChange(event.target.value)
                      }
                      className="h-9 w-full rounded-md border border-slate-200 bg-white px-2 text-sm font-medium text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200 disabled:cursor-not-allowed disabled:bg-slate-100"
                    >
                      <option value="">
                        {sprintsQuery.isLoading
                          ? "Loading sprints..."
                          : sprintsQuery.isError
                            ? "Unable to load sprints"
                            : "Select a sprint"}
                      </option>

                      {(sprintsQuery.data?.items ?? []).map(
                        (sprint) => (
                          <option
                            key={sprint.sprintId}
                            value={sprint.sprintId}
                          >
                            {sprint.name}
                          </option>
                        ),
                      )}
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
                      {typeof workItem.type === "string"
                        ? workItem.type
                        : "Work item"}
                    </dd>
                  </div>

                  <div className="flex justify-between gap-3">
                    <dt className="text-slate-500">Status</dt>
                    <dd className="font-medium text-slate-700">
                      {labelFor(
                        selectedStatus,
                        statusOptions,
                        "To do",
                      )}
                    </dd>
                  </div>

                  <div className="flex justify-between gap-3">
                    <dt className="text-slate-500">Priority</dt>
                    <dd className="font-medium text-slate-700">
                      {labelFor(
                        selectedPriority,
                        priorityOptions,
                        "Medium",
                      )}
                    </dd>
                  </div>

                  {assignee && (
                    <div className="flex items-center justify-between gap-3">
                      <dt className="text-slate-500">
                        Assigned to
                      </dt>

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