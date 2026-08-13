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
  ProjectVisibility,
  type ProjectDetail,
} from "../api/projects-api";
import {
  useArchiveProject,
  useCreateProject,
  useUpdateProject,
} from "../hooks/use-project-mutations";

type ProjectDialogProps =
  | {
      mode: "create";
      open: boolean;
      onOpenChange: (open: boolean) => void;
      project?: never;
      onCreated?: (projectId: string) => void;
    }
  | {
      mode: "edit";
      open: boolean;
      onOpenChange: (open: boolean) => void;
      project: ProjectDetail;
      onCreated?: never;
    };

type FormValues = {
  key: string;
  name: string;
  description: string;
  visibility: ProjectVisibility;
};

const initialValues: FormValues = {
  key: "",
  name: "",
  description: "",
  visibility: ProjectVisibility.Private,
};

function visibilityFromApi(
  visibility: string,
): ProjectVisibility {
  switch (visibility.toLowerCase()) {
    case "internal":
      return ProjectVisibility.Internal;
    case "public":
      return ProjectVisibility.Public;
    default:
      return ProjectVisibility.Private;
  }
}

export function ProjectDialog(
  props: ProjectDialogProps,
) {
  const createProject = useCreateProject();
  const updateProject = useUpdateProject();
  const archiveProject = useArchiveProject();

  const [values, setValues] =
    useState<FormValues>(initialValues);
  const [error, setError] = useState<string | null>(
    null,
  );
  const [showArchiveConfirm, setShowArchiveConfirm] =
    useState(false);

  useEffect(() => {
    if (!props.open) {
      return;
    }

    setError(null);
    setShowArchiveConfirm(false);

    if (props.mode === "edit") {
      setValues({
        key: props.project.key,
        name: props.project.name,
        description: props.project.description ?? "",
        visibility: visibilityFromApi(
          props.project.visibility,
        ),
      });
      return;
    }

    setValues(initialValues);
  }, [props]);

  const isSubmitting =
    createProject.isPending ||
    updateProject.isPending ||
    archiveProject.isPending;

  function updateValue<Key extends keyof FormValues>(
    key: Key,
    value: FormValues[Key],
  ) {
    setValues((current) => ({
      ...current,
      [key]: value,
    }));
  }

  async function handleSubmit(
    event: React.FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    const name = values.name.trim();
    const key = values.key.trim().toUpperCase();

    if (!name) {
      setError("Project name is required.");
      return;
    }

    if (props.mode === "create" && !key) {
      setError("Project key is required.");
      return;
    }

    setError(null);

    try {
      if (props.mode === "create") {
        const project = await createProject.mutateAsync({
          key,
          name,
          description:
            values.description.trim() || null,
          visibility: values.visibility,
        });

        props.onOpenChange(false);
        props.onCreated?.(project.projectId);
        return;
      }

      await updateProject.mutateAsync({
        projectId: props.project.projectId,
        request: {
          name,
          description:
            values.description.trim() || null,
          visibility: values.visibility,
        },
      });

      props.onOpenChange(false);
    } catch {
      setError(
        "Unable to save the project. Please review the fields and try again.",
      );
    }
  }

  async function handleArchive() {
    if (props.mode !== "edit") {
      return;
    }

    setError(null);

    try {
      await archiveProject.mutateAsync(
        props.project.projectId,
      );

      props.onOpenChange(false);
    } catch {
      setError(
        "Unable to archive this project. Please try again.",
      );
    }
  }

  const title =
    props.mode === "create"
      ? "Create project"
      : "Edit project";

  return (
    <Dialog
      open={props.open}
      onOpenChange={props.onOpenChange}
    >
      <DialogContent className="max-h-[90vh] overflow-y-auto sm:max-w-lg">
        <DialogHeader>
          <DialogTitle>{title}</DialogTitle>

          <DialogDescription>
            {props.mode === "create"
              ? "Set up a new workspace for your team."
              : "Update the project details or archive it."}
          </DialogDescription>
        </DialogHeader>

        {showArchiveConfirm ? (
          <div className="space-y-5 px-6 py-5">
            <div className="rounded-lg border border-red-200 bg-red-50 p-4">
              <p className="font-medium text-red-800">
                Archive this project?
              </p>

              <p className="mt-1 text-sm text-red-700">
                The project will be hidden from normal project
                lists. It can be restored later.
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
                onClick={() => setShowArchiveConfirm(false)}
              >
                Cancel
              </Button>

              <Button
                type="button"
                variant="destructive"
                disabled={isSubmitting}
                onClick={handleArchive}
              >
                {archiveProject.isPending && (
                  <LoaderCircle className="h-4 w-4 animate-spin" />
                )}
                Archive project
              </Button>
            </DialogFooter>
          </div>
        ) : (
          <form
            className="space-y-5 px-6 py-5"
            onSubmit={handleSubmit}
          >
            {props.mode === "create" && (
              <div className="space-y-2">
                <Label htmlFor="project-key">
                  Project key
                </Label>

                <Input
                  id="project-key"
                  value={values.key}
                  maxLength={12}
                  autoComplete="off"
                  placeholder="e.g. DEV"
                  onChange={(event) =>
                    updateValue(
                      "key",
                      event.target.value
                        .replace(/[^a-z0-9]/gi, "")
                        .toUpperCase(),
                    )
                  }
                />

                <p className="text-xs text-slate-500">
                  A short, unique identifier used in work item
                  keys.
                </p>
              </div>
            )}

            <div className="space-y-2">
              <Label htmlFor="project-name">Name</Label>

              <Input
                id="project-name"
                value={values.name}
                maxLength={150}
                placeholder="e.g. Website redesign"
                onChange={(event) =>
                  updateValue("name", event.target.value)
                }
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="project-description">
                Description
              </Label>

              <textarea
                id="project-description"
                value={values.description}
                maxLength={2000}
                rows={4}
                placeholder="What is this project for?"
                onChange={(event) =>
                  updateValue(
                    "description",
                    event.target.value,
                  )
                }
                className="flex w-full rounded-lg border border-slate-200 bg-white px-3 py-2 text-sm outline-none placeholder:text-slate-400 focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="project-visibility">
                Visibility
              </Label>

              <select
                id="project-visibility"
                value={values.visibility}
                onChange={(event) =>
                  updateValue(
                    "visibility",
                    Number(event.target.value) as ProjectVisibility,
                  )
                }
                className="flex h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
              >
                <option value={ProjectVisibility.Private}>
                  Private
                </option>
                <option value={ProjectVisibility.Internal}>
                  Internal
                </option>
                <option value={ProjectVisibility.Public}>
                  Public
                </option>
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
                  onClick={() => setShowArchiveConfirm(true)}
                  className="mr-auto"
                >
                  Archive
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
                  ? "Create project"
                  : "Save changes"}
              </Button>
            </DialogFooter>
          </form>
        )}
      </DialogContent>
    </Dialog>
  );
}