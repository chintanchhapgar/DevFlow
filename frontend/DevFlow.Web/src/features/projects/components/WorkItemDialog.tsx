import { useEffect, useState } from "react";
import { LoaderCircle } from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

import {
  WorkItemPriority,
  WorkItemType,
  type WorkItem,
} from "../api/project-resources-api";
import {
  useCreateWorkItem,
  useDeleteWorkItem,
  useMoveWorkItemToSprint,
  useUpdateWorkItem,
} from "../hooks/use-project-resources";
import { useProjectSprints } from "../hooks/use-sprints";

type WorkItemDialogProps =
  | {
      mode: "create";
      open: boolean;
      onOpenChange: (open: boolean) => void;
      projectId: string;
      workItem?: never;
    }
  | {
      mode: "edit";
      open: boolean;
      onOpenChange: (open: boolean) => void;
      projectId: string;
      workItem: WorkItem;
    };

type FormValues = {
  title: string;
  description: string;
  type: WorkItemType;
  priority: WorkItemPriority;
  dueDate: string;
  estimateHours: string;
};

const initialValues: FormValues = {
  title: "",
  description: "",
  type: WorkItemType.Task,
  priority: WorkItemPriority.Medium,
  dueDate: "",
  estimateHours: "",
};

function asNumber<T extends number>(
  value: string | number,
  fallback: T,
): T {
  return (
    typeof value === "number" ? value : fallback
  ) as T;
}

function toDateInputValue(
  value: string | null | undefined,
) {
  return value ? value.slice(0, 10) : "";
}

export function WorkItemDialog(
  props: WorkItemDialogProps,
) {
  const createWorkItem = useCreateWorkItem();
  const updateWorkItem = useUpdateWorkItem();
  const deleteWorkItem = useDeleteWorkItem();
  const moveWorkItemToSprint = useMoveWorkItemToSprint();
  const sprintsQuery = useProjectSprints(props.projectId);

  const [values, setValues] =
    useState<FormValues>(initialValues);
  const [error, setError] = useState<string | null>(null);
  const [showDeleteConfirm, setShowDeleteConfirm] =
    useState(false);
  const [selectedSprintId, setSelectedSprintId] = useState("");

  useEffect(() => {
    if (!props.open) {
      return;
    }

    setError(null);
    setShowDeleteConfirm(false);

    if (props.mode === "edit") {
      setValues({
        title: props.workItem.title,
        description: props.workItem.description ?? "",
        type: asNumber(
          props.workItem.type,
          WorkItemType.Task,
        ),
        priority: asNumber(
          props.workItem.priority,
          WorkItemPriority.Medium,
        ),
        dueDate: toDateInputValue(
          props.workItem.dueDate,
        ),
        estimateHours:
          props.workItem.estimateHours?.toString() ?? "",
      });
      return;
    }

    setValues(initialValues);
    setSelectedSprintId("");
  }, [props]);


  useEffect(() => {
  if (!props.open || !sprintsQuery.data) return;

  if (props.mode === "edit") {
    setSelectedSprintId(props.workItem.sprintId ?? "");
  }
}, [props.open, props.mode, 
    props.mode === "edit" ? props.workItem?.sprintId : null, 
    sprintsQuery.data]);

  const isSubmitting =
    createWorkItem.isPending ||
    updateWorkItem.isPending ||
    deleteWorkItem.isPending ||
    moveWorkItemToSprint.isPending;

  function setValue<Key extends keyof FormValues>(
    key: Key,
    value: FormValues[Key],
  ) {
    setValues((current) => ({
      ...current,
      [key]: value,
    }));
  }

  function requestBody() {
    const hours = values.estimateHours.trim();

    return {
      title: values.title.trim(),
      description: values.description.trim() || null,
      dueDate: values.dueDate
            ? `${values.dueDate}T00:00:00.000Z`
            : null,
      estimateHours: hours ? Number(hours) : null,
    };
  }

  async function handleSubmit(
    event: React.FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    if (!values.title.trim()) {
      setError("Work item title is required.");
      return;
    }

    if (
      values.estimateHours.trim() &&
      (!Number.isFinite(Number(values.estimateHours)) ||
        Number(values.estimateHours) < 0)
    ) {
      setError(
        "Estimated hours must be a positive number.",
      );
      return;
    }

    setError(null);

    try {
      const result = props.mode === "create"
        ? await createWorkItem.mutateAsync({
          projectId: props.projectId,
          request: {
            ...requestBody(),
            type: values.type,
            priority: values.priority,
            assigneeId: null,
          },
        })
        : await updateWorkItem.mutateAsync({
          projectId: props.projectId,
          workItemId: props.workItem.id,
          request: requestBody(),
        });

      if (selectedSprintId) {
        await moveWorkItemToSprint.mutateAsync({
          projectId: props.projectId,
          workItemId: result.workItemId,
          sprintId: selectedSprintId,
        });
      }

      props.onOpenChange(false);
    } catch {
      setError(
        "Unable to save the work item. Please try again.",
      );
    }
  }

  async function handleDelete() {
    if (props.mode !== "edit") {
      return;
    }

    setError(null);

    try {
      await deleteWorkItem.mutateAsync({
        projectId: props.projectId,
        workItemId: props.workItem.id,
      });

      props.onOpenChange(false);
    } catch {
      setError("Unable to delete this work item.");
    }
  }

  return (
    <Dialog
      open={props.open}
      onOpenChange={props.onOpenChange}
    >
      <DialogContent className="max-h-[90vh] overflow-y-auto sm:max-w-lg">
        <DialogHeader>
          <DialogTitle>
            {props.mode === "create"
              ? "Create work item"
              : "Edit work item"}
          </DialogTitle>

          <DialogDescription>
            {props.mode === "create"
              ? "Add a task, bug, story, or other item to this project."
              : "Update the editable fields for this work item."}
          </DialogDescription>
        </DialogHeader>

        {showDeleteConfirm ? (
          <div className="space-y-5 px-6 py-5">
            <div className="rounded-lg border border-red-200 bg-red-50 p-4">
              <p className="font-medium text-red-800">
                Delete this work item?
              </p>

              <p className="mt-1 text-sm text-red-700">
                This action cannot be undone.
              </p>
            </div>

            {error && (
              <p className="text-sm text-red-600">{error}</p>
            )}

            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                disabled={isSubmitting}
                onClick={() => setShowDeleteConfirm(false)}
              >
                Cancel
              </Button>

              <Button
                type="button"
                variant="destructive"
                disabled={isSubmitting}
                onClick={handleDelete}
              >
                {deleteWorkItem.isPending && (
                  <LoaderCircle className="h-4 w-4 animate-spin" />
                )}
                Delete work item
              </Button>
            </DialogFooter>
          </div>
        ) : (
          <form
            className="space-y-5 px-6 py-5"
            onSubmit={handleSubmit}
          >
            <div className="space-y-2">
              <Label htmlFor="work-title">Title</Label>

              <Input
                id="work-title"
                value={values.title}
                maxLength={250}
                placeholder="e.g. Implement project search"
                onChange={(event) =>
                  setValue("title", event.target.value)
                }
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="work-description">
                Description
              </Label>

              <textarea
                id="work-description"
                value={values.description}
                rows={4}
                maxLength={5000}
                placeholder="Add context, acceptance criteria, or notes."
                onChange={(event) =>
                  setValue(
                    "description",
                    event.target.value,
                  )
                }
                className="flex w-full rounded-lg border border-slate-200 bg-white px-3 py-2 text-sm outline-none placeholder:text-slate-400 focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
              />
            </div>

            {props.mode === "create" && (
              <div className="grid gap-4 sm:grid-cols-2">
                <div className="space-y-2">
                  <Label htmlFor="work-type">Type</Label>

                  <select
                    id="work-type"
                    value={values.type}
                    onChange={(event) =>
                      setValue(
                        "type",
                        Number(
                          event.target.value,
                        ) as WorkItemType,
                      )
                    }
                    className="flex h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                  >
                    <option value={WorkItemType.Task}>
                      Task
                    </option>
                    <option value={WorkItemType.Bug}>
                      Bug
                    </option>
                    <option value={WorkItemType.Story}>
                      Story
                    </option>
                    <option value={WorkItemType.Epic}>
                      Epic
                    </option>
                    <option value={WorkItemType.Subtask}>
                      Subtask
                    </option>
                  </select>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="work-priority">
                    Priority
                  </Label>

                  <select
                    id="work-priority"
                    value={values.priority}
                    onChange={(event) =>
                      setValue(
                        "priority",
                        Number(
                          event.target.value,
                        ) as WorkItemPriority,
                      )
                    }
                    className="flex h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                  >
                    <option value={WorkItemPriority.Lowest}>
                      Lowest
                    </option>
                    <option value={WorkItemPriority.Low}>
                      Low
                    </option>
                    <option value={WorkItemPriority.Medium}>
                      Medium
                    </option>
                    <option value={WorkItemPriority.High}>
                      High
                    </option>
                    <option value={WorkItemPriority.Highest}>
                      Highest
                    </option>
                  </select>
                </div>
              </div>
            )}

            <div className="grid gap-4 sm:grid-cols-2">
              <div className="space-y-2">
                <Label htmlFor="work-due-date">
                  Due date
                </Label>

                <Input
                  id="work-due-date"
                  type="date"
                  value={values.dueDate}
                  onChange={(event) =>
                    setValue(
                      "dueDate",
                      event.target.value,
                    )
                  }
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="work-estimate">
                  Estimated hours
                </Label>

                <Input
                  id="work-estimate"
                  type="number"
                  min="0"
                  step="0.25"
                  value={values.estimateHours}
                  placeholder="e.g. 4"
                  onChange={(event) =>
                    setValue(
                      "estimateHours",
                      event.target.value,
                    )
                  }
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="work-sprint">Sprint</Label>

              <select
                id="work-sprint"
                value={selectedSprintId}
                disabled={sprintsQuery.isLoading || sprintsQuery.isError}
                onChange={(event) =>
                  setSelectedSprintId(event.target.value)
                }
                className="flex h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
              >
                <option value="">
                  {sprintsQuery.isLoading
                    ? "Loading sprints..."
                    : sprintsQuery.isError
                      ? "Unable to load sprints"
                      : "Backlog (no sprint)"}
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
            </div>

            {error && (
              <p className="text-sm text-red-600">{error}</p>
            )}

            <DialogFooter>
              {props.mode === "edit" && (
                <Button
                  type="button"
                  variant="destructive"
                  disabled={isSubmitting}
                  onClick={() => setShowDeleteConfirm(true)}
                  className="mr-auto"
                >
                  Delete
                </Button>
              )}

              <Button
                type="button"
                variant="outline"
                disabled={isSubmitting}
                onClick={() => props.onOpenChange(false)}
              >
                Cancel
              </Button>

              <Button type="submit" disabled={isSubmitting}>
                {isSubmitting && (
                  <LoaderCircle className="h-4 w-4 animate-spin" />
                )}
                {props.mode === "create"
                  ? "Create work item"
                  : "Save changes"}
              </Button>
            </DialogFooter>
          </form>
        )}
      </DialogContent>
    </Dialog>
  );
}
