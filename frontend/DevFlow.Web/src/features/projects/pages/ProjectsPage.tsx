import { useState } from "react";
import {
  ChevronLeft,
  ChevronRight,
  FolderKanban,
  Plus,
  Search,
  Users,
} from "lucide-react";
import { Link, useNavigate } from "react-router-dom";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

import { ProjectDialog } from "../components/ProjectDialog";
import { useDebounce } from "../hooks/use-debounce";
import { useProjects } from "../hooks/use-projects";

const PAGE_SIZE = 20;

export function ProjectsPage() {
  const navigate = useNavigate();

  const [page, setPage] = useState(1);
  const [searchInput, setSearchInput] = useState("");
  const [isCreateOpen, setIsCreateOpen] = useState(false);

  const search = useDebounce(searchInput.trim(), 350);

  const projectsQuery = useProjects({
    page,
    pageSize: PAGE_SIZE,
    search: search || undefined,
  });

  const projects = projectsQuery.data?.items ?? [];
  const totalCount = projectsQuery.data?.totalCount ?? 0;
  const totalPages = projectsQuery.data?.totalPages ?? 0;

  function clearSearch() {
    setSearchInput("");
    setPage(1);
  }

  return (
    <div className="mx-auto w-full max-w-7xl space-y-6">
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div className="flex items-center gap-3">
          <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-slate-50 text-slate-600 ring-1 ring-slate-200">
            <FolderKanban className="h-5 w-5" />
          </div>

          <div>
            <h1 className="text-2xl font-semibold tracking-tight text-slate-900">
              Projects
            </h1>

            <p className="mt-1 text-sm text-slate-500">
              Manage and organize your projects.
            </p>
          </div>
        </div>

        <Button
          type="button"
          onClick={() => setIsCreateOpen(true)}
        >
          <Plus className="h-4 w-4" />
          New project
        </Button>
      </div>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <div className="flex flex-col gap-4 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between">
          <div className="relative w-full sm:max-w-sm">
            <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />

            <Input
              type="search"
              value={searchInput}
              placeholder="Search projects..."
              className="pl-9"
              onChange={(event) => {
                setSearchInput(event.target.value);
                setPage(1);
              }}
            />
          </div>

          <p className="text-sm text-slate-500">
            {projectsQuery.isFetching ? (
              "Loading..."
            ) : (
              <>
                <span className="font-medium text-slate-700">
                  {totalCount}
                </span>{" "}
                {totalCount === 1 ? "project" : "projects"}
              </>
            )}
          </p>
        </div>

        {projectsQuery.isError && (
          <div className="m-5 rounded-lg border border-red-200 bg-red-50 p-4">
            <p className="font-medium text-red-800">
              Unable to load projects.
            </p>

            <p className="mt-1 text-sm text-red-700">
              Check your connection and try again.
            </p>

            <Button
              type="button"
              variant="outline"
              size="sm"
              className="mt-3"
              onClick={() => projectsQuery.refetch()}
            >
              Try again
            </Button>
          </div>
        )}

        {projectsQuery.isLoading && (
          <div className="divide-y divide-slate-100">
            {Array.from({ length: 6 }).map((_, index) => (
              <div
                key={index}
                className="flex items-center gap-4 px-5 py-4"
              >
                <div className="h-10 w-10 animate-pulse rounded-xl bg-slate-100" />

                <div className="flex-1 space-y-2">
                  <div className="h-4 w-44 animate-pulse rounded bg-slate-100" />
                  <div className="h-3 w-28 animate-pulse rounded bg-slate-100" />
                </div>
              </div>
            ))}
          </div>
        )}

        {!projectsQuery.isLoading &&
          !projectsQuery.isError &&
          projects.length === 0 && (
            <div className="flex min-h-[320px] flex-col items-center justify-center px-5 text-center">
              <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-slate-50 text-slate-400 ring-1 ring-slate-200">
                <FolderKanban className="h-6 w-6" />
              </div>

              <h2 className="mt-4 text-sm font-semibold text-slate-900">
                No projects found
              </h2>

              <p className="mt-1 max-w-sm text-sm text-slate-500">
                {search
                  ? `No projects match "${search}".`
                  : "Create your first project to get started."}
              </p>

              <Button
                type="button"
                variant={search ? "outline" : "default"}
                size="sm"
                className="mt-4"
                onClick={
                  search
                    ? clearSearch
                    : () => setIsCreateOpen(true)
                }
              >
                {search ? "Clear search" : "New project"}
              </Button>
            </div>
          )}

        {!projectsQuery.isLoading &&
          !projectsQuery.isError &&
          projects.length > 0 && (
            <>
              <div className="hidden grid-cols-[minmax(260px,1fr)_120px_120px_100px] gap-4 border-b border-slate-100 bg-slate-50 px-5 py-3 text-[11px] font-semibold uppercase tracking-wider text-slate-400 md:grid">
                <span>Project</span>
                <span>Status</span>
                <span>Visibility</span>
                <span>Members</span>
              </div>

              <div className="divide-y divide-slate-100">
                {projects.map((project) => (
                  <Link
                    key={project.projectId}
                    to={`/projects/${project.projectId}`}
                    className="grid gap-3 px-5 py-4 transition-colors hover:bg-slate-50 md:grid-cols-[minmax(260px,1fr)_120px_120px_100px] md:items-center md:gap-4"
                  >
                    <div className="flex min-w-0 items-center gap-3">
                      <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-slate-100 text-sm font-semibold text-slate-600">
                        {project.key.slice(0, 2).toUpperCase()}
                      </div>

                      <div className="min-w-0">
                        <p className="truncate text-sm font-medium text-slate-900">
                          {project.name}
                        </p>

                        <p className="mt-0.5 text-xs text-slate-500">
                          {project.key}
                        </p>
                      </div>
                    </div>

                    <ProjectPill value={project.status} />

                    <ProjectPill value={project.visibility} />

                    <span className="flex items-center gap-1.5 text-sm text-slate-600">
                      <Users className="h-4 w-4 text-slate-400" />
                      {project.memberCount}
                    </span>
                  </Link>
                ))}
              </div>
            </>
          )}

        {!projectsQuery.isLoading &&
          !projectsQuery.isError &&
          totalPages > 1 && (
            <div className="flex items-center justify-between border-t border-slate-100 px-5 py-4">
              <p className="text-xs text-slate-500">
                Page {page} of {totalPages}
              </p>

              <div className="flex gap-2">
                <Button
                  type="button"
                  variant="outline"
                  size="sm"
                  disabled={
                    !projectsQuery.data?.hasPreviousPage ||
                    projectsQuery.isFetching
                  }
                  onClick={() =>
                    setPage((current) =>
                      Math.max(1, current - 1),
                    )
                  }
                >
                  <ChevronLeft className="h-4 w-4" />
                  Previous
                </Button>

                <Button
                  type="button"
                  variant="outline"
                  size="sm"
                  disabled={
                    !projectsQuery.data?.hasNextPage ||
                    projectsQuery.isFetching
                  }
                  onClick={() =>
                    setPage((current) => current + 1)
                  }
                >
                  Next
                  <ChevronRight className="h-4 w-4" />
                </Button>
              </div>
            </div>
          )}
      </section>

      <ProjectDialog
        mode="create"
        open={isCreateOpen}
        onOpenChange={setIsCreateOpen}
        onCreated={(projectId) =>
          navigate(`/projects/${projectId}`)
        }
      />
    </div>
  );
}

function ProjectPill({ value }: { value: string }) {
  return (
    <span className="w-fit rounded-full bg-slate-100 px-2.5 py-1 text-xs font-medium text-slate-600">
      {value}
    </span>
  );
}