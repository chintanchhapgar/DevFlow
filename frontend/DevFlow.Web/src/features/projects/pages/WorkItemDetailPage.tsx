import { useQuery } from "@tanstack/react-query";
import {
  AlertCircle,
  ArrowLeft,
  LoaderCircle,
} from "lucide-react";
import {
  Link,
  useNavigate,
  useParams,
} from "react-router-dom";

import { Button } from "@/components/ui/button";

import {
  getWorkItem,
} from "../api/project-resources-api";
import { WorkItemDetailDialog } from "../components/WorkItemDetailDialog";
import { useProject } from "../hooks/use-project";

export function WorkItemDetailPage() {
  const navigate = useNavigate();

  const {
    projectId,
    workItemId,
  } = useParams<{
    projectId: string;
    workItemId: string;
  }>();

  const projectQuery = useProject(projectId);

  const workItemQuery = useQuery({
    queryKey: ["work-item", workItemId],

    queryFn: () => {
      if (!workItemId) {
        throw new Error("Work item ID is required.");
      }

      return getWorkItem(workItemId);
    },

    enabled: Boolean(workItemId),
    staleTime: 30_000,
    refetchOnWindowFocus: false,
  });

  function returnToProject() {
    navigate(`/projects/${projectId}`);
  }

  if (projectQuery.isLoading || workItemQuery.isLoading) {
    return (
      <div className="flex min-h-[55vh] items-center justify-center">
        <div className="flex items-center gap-3 text-sm text-slate-500">
          <LoaderCircle className="h-5 w-5 animate-spin text-[var(--devflow-primary)]" />
          Loading work item...
        </div>
      </div>
    );
  }

  if (
    projectQuery.isError ||
    workItemQuery.isError ||
    !projectQuery.data ||
    !workItemQuery.data
  ) {
    return (
      <div className="mx-auto flex w-full max-w-3xl flex-col items-center justify-center rounded-2xl border border-red-200 bg-red-50 px-5 py-16 text-center">
        <AlertCircle className="h-8 w-8 text-red-600" />

        <h1 className="mt-3 text-lg font-semibold text-red-900">
          Unable to load work item
        </h1>

        <p className="mt-1 max-w-md text-sm text-red-700">
          This work item may not exist, belong to another project, or you
          may not have access to it.
        </p>

        <div className="mt-5 flex gap-2">
          <Button
            type="button"
            variant="outline"
            onClick={() => {
              void projectQuery.refetch();
              void workItemQuery.refetch();
            }}
          >
            Try again
          </Button>

          <Button type="button" onClick={returnToProject}>
            Back to project
          </Button>
        </div>
      </div>
    );
  }

  const project = projectQuery.data;
  const workItem = workItemQuery.data;

  return (
    <div className="mx-auto w-full max-w-7xl">
      <Link
        to={`/projects/${projectId}`}
        className="inline-flex items-center gap-2 rounded-lg px-2 py-1.5 text-sm font-medium text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900"
      >
        <ArrowLeft className="h-4 w-4" />
        Back to {project.name}
      </Link>

      <div className="mt-6 rounded-2xl border border-slate-200 bg-white px-5 py-10 text-center shadow-sm">
        <p className="text-sm font-medium text-slate-700">
          Opening work item details…
        </p>
      </div>

      <WorkItemDetailDialog
        open
        onOpenChange={(open) => {
          if (!open) {
            returnToProject();
          }
        }}
        projectId={projectId}
        workItem={workItem}
        members={project.members ?? []}
      />
    </div>
  );
}