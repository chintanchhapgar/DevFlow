import { useEffect, useState } from "react";
import {
  FolderKanban,
  Plus,
  Search,
  Users,
  ChevronLeft,
  ChevronRight,
  MoreHorizontal,
} from "lucide-react";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

import {
  getProjects,
  type ProjectListItem,
} from "@/features/projects/api/projects-api";

const PAGE_SIZE = 20;

export function ProjectsPage() {
  const [projects, setProjects] = useState<ProjectListItem[]>(
    [],
  );

  const [page, setPage] = useState(1);
  const [pageSize] = useState(PAGE_SIZE);

  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);

  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] =
    useState(false);

  const [searchInput, setSearchInput] = useState("");
  const [search, setSearch] = useState("");

  const [isLoading, setIsLoading] = useState(true);
  const [isError, setIsError] = useState(false);

  /*
   * Debounce search input.
   *
   * This prevents an API request on every keystroke
   * and keeps the search box responsive.
   */
  useEffect(() => {
    const timer = window.setTimeout(() => {
      setSearch(searchInput.trim());
      setPage(1);
    }, 350);

    return () => {
      window.clearTimeout(timer);
    };
  }, [searchInput]);

  /*
   * Load projects.
   */
  useEffect(() => {
    let cancelled = false;

    async function loadProjects() {
      setIsLoading(true);
      setIsError(false);

      try {
        const result = await getProjects({
          page,
          pageSize,
          search: search || undefined,
        });

        if (cancelled) {
          return;
        }

        setProjects(result.items);
        setTotalCount(result.totalCount);
        setTotalPages(result.totalPages);
        setHasNextPage(result.hasNextPage);
        setHasPreviousPage(
          result.hasPreviousPage,
        );
      } catch {
        if (!cancelled) {
          setProjects([]);
          setIsError(true);
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    }

    loadProjects();

    return () => {
      cancelled = true;
    };
  }, [page, pageSize, search]);

  function handlePreviousPage() {
    if (!hasPreviousPage || isLoading) {
      return;
    }

    setPage((current) => Math.max(1, current - 1));
  }

  function handleNextPage() {
    if (!hasNextPage || isLoading) {
      return;
    }

    setPage((current) => current + 1);
  }

  function handleNewProject() {
    // TODO:
    // Navigate to the project creation page
    // when the create-project route is available.
  }

  function getProjectInitials(project: ProjectListItem) {
    if (project.key) {
      return project.key
        .slice(0, 2)
        .toUpperCase();
    }

    return project.name
      .slice(0, 2)
      .toUpperCase();
  }

  return (
    <div className="mx-auto w-full max-w-7xl space-y-6">
      {/* ─────────────────────────────────────
          Page Header
      ───────────────────────────────────── */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <div className="flex items-center gap-3">
            <div
              className="
                flex
                h-10
                w-10
                items-center
                justify-center
                rounded-lg
                bg-[#eef3f8]
                text-[#456b9a]
                ring-1
                ring-[#dbe4ed]
              "
            >
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
        </div>

        <Button
          type="button"
          onClick={handleNewProject}
        >
          <Plus className="h-4 w-4" />
          New Project
        </Button>
      </div>

      {/* ─────────────────────────────────────
          Main Card
      ───────────────────────────────────── */}
      <section
        className="
          overflow-hidden
          rounded-xl
          border
          border-slate-200
          bg-white
          shadow-sm
        "
      >
        {/* Toolbar */}
        <div
          className="
            flex
            flex-col
            gap-4
            border-b
            border-slate-200
            px-5
            py-4
            sm:flex-row
            sm:items-center
            sm:justify-between
          "
        >
          {/* Search */}
          <div className="relative w-full sm:max-w-sm">
            <Search
              className="
                pointer-events-none
                absolute
                left-3
                top-1/2
                h-4
                w-4
                -translate-y-1/2
                text-slate-400
              "
            />

            <Input
              type="search"
              value={searchInput}
              onChange={(event) =>
                setSearchInput(event.target.value)
              }
              placeholder="Search projects..."
              className="
                h-10
                border-slate-200
                bg-white
                pl-9
                pr-9
                text-slate-900
                placeholder:text-slate-400
                focus-visible:border-[#9db2c8]
                focus-visible:ring-[#9db2c8]
              "
            />

            {isLoading && (
              <div
                className="
                  absolute
                  right-3
                  top-1/2
                  h-4
                  w-4
                  -translate-y-1/2
                  animate-spin
                  rounded-full
                  border-2
                  border-slate-200
                  border-t-[#456b9a]
                "
              />
            )}
          </div>

          {/* Count */}
          <div className="text-sm text-slate-500">
            {isLoading ? (
              "Loading..."
            ) : (
              <>
                <span className="font-medium text-slate-700">
                  {totalCount}
                </span>{" "}
                {totalCount === 1
                  ? "project"
                  : "projects"}
              </>
            )}
          </div>
        </div>

        {/* Error */}
        {isError && (
          <div className="border-b border-red-100 bg-red-50 px-5 py-4">
            <p className="text-sm font-medium text-red-600">
              Unable to load projects.
            </p>

            <p className="mt-1 text-xs text-red-500">
              Please try again.
            </p>
          </div>
        )}

        {/* Loading */}
        {isLoading && projects.length === 0 && (
          <div className="divide-y divide-slate-100">
            {Array.from({ length: 6 }).map(
              (_, index) => (
                <div
                  key={index}
                  className="flex items-center gap-4 px-5 py-4"
                >
                  <div className="h-10 w-10 animate-pulse rounded-lg bg-slate-100" />

                  <div className="min-w-0 flex-1 space-y-2">
                    <div className="h-4 w-40 animate-pulse rounded bg-slate-100" />

                    <div className="h-3 w-24 animate-pulse rounded bg-slate-100" />
                  </div>

                  <div className="hidden h-4 w-20 animate-pulse rounded bg-slate-100 sm:block" />
                </div>
              ),
            )}
          </div>
        )}

        {/* Empty */}
        {!isLoading &&
          !isError &&
          projects.length === 0 && (
            <div className="flex min-h-[320px] flex-col items-center justify-center px-5 text-center">
              <div
                className="
                  flex
                  h-12
                  w-12
                  items-center
                  justify-center
                  rounded-xl
                  bg-slate-50
                  text-slate-400
                "
              >
                <FolderKanban className="h-6 w-6" />
              </div>

              <h3 className="mt-4 text-sm font-semibold text-slate-900">
                No projects found
              </h3>

              <p className="mt-1 max-w-sm text-sm text-slate-500">
                {search
                  ? `No projects match "${search}".`
                  : "Create your first project to get started."}
              </p>

              {search && (
                <Button
                  type="button"
                  variant="outline"
                  size="sm"
                  className="mt-4"
                  onClick={() => {
                    setSearchInput("");
                    setSearch("");
                    setPage(1);
                  }}
                >
                  Clear search
                </Button>
              )}
            </div>
          )}

        {/* Project List */}
        {!isError && projects.length > 0 && (
          <>
            {/* Desktop Header */}
            <div
              className="
                hidden
                grid-cols-[minmax(280px,1fr)_120px_120px_100px_48px]
                gap-4
                border-b
                border-slate-100
                bg-slate-50/70
                px-5
                py-3
                text-[11px]
                font-semibold
                uppercase
                tracking-wider
                text-slate-400
                md:grid
              "
            >
              <span>Project</span>
              <span>Status</span>
              <span>Visibility</span>
              <span>Members</span>
              <span />
            </div>

            <div className="divide-y divide-slate-100">
              {projects.map((project) => (
                <div
                  key={project.projectId}
                  className="
                    group
                    px-5
                    py-4
                    transition-colors
                    hover:bg-slate-50/70
                  "
                >
                  <div
                    className="
                      grid
                      gap-4
                      md:grid-cols-[minmax(280px,1fr)_120px_120px_100px_48px]
                      md:items-center
                    "
                  >
                    {/* Project */}
                    <div className="flex min-w-0 items-center gap-3">
                      <div
                        className="
                          flex
                          h-10
                          w-10
                          shrink-0
                          items-center
                          justify-center
                          rounded-lg
                          bg-[#eef3f8]
                          text-xs
                          font-semibold
                          text-[#456b9a]
                          ring-1
                          ring-[#dbe4ed]
                        "
                      >
                        {getProjectInitials(project)}
                      </div>

                      <div className="min-w-0">
                        <div className="flex items-center gap-2">
                          <h3 className="truncate text-sm font-semibold text-slate-900">
                            {project.name}
                          </h3>

                          <span className="shrink-0 rounded bg-slate-100 px-1.5 py-0.5 text-[10px] font-semibold text-slate-500">
                            {project.key}
                          </span>
                        </div>

                        <p className="mt-1 truncate text-xs text-slate-400">
                          Project ID:{" "}
                          {project.projectId}
                        </p>

                        {/* Mobile metadata */}
                        <div className="mt-2 flex flex-wrap items-center gap-2 md:hidden">
                          <ProjectStatus
                            status={project.status}
                          />

                          <ProjectVisibility
                            visibility={
                              project.visibility
                            }
                          />

                          <span className="inline-flex items-center gap-1 text-xs text-slate-500">
                            <Users className="h-3.5 w-3.5" />
                            {project.memberCount}
                          </span>
                        </div>
                      </div>
                    </div>

                    {/* Status */}
                    <div className="hidden md:block">
                      <ProjectStatus
                        status={project.status}
                      />
                    </div>

                    {/* Visibility */}
                    <div className="hidden md:block">
                      <ProjectVisibility
                        visibility={project.visibility}
                      />
                    </div>

                    {/* Members */}
                    <div className="hidden md:flex items-center gap-1.5 text-sm text-slate-600">
                      <Users className="h-4 w-4 text-slate-400" />

                      <span>
                        {project.memberCount}
                      </span>
                    </div>

                    {/* Actions */}
                    <div className="hidden justify-end md:flex">
                      <button
                        type="button"
                        aria-label={`More options for ${project.name}`}
                        className="
                          flex
                          h-8
                          w-8
                          items-center
                          justify-center
                          rounded-md
                          text-slate-400
                          opacity-0
                          transition
                          hover:bg-slate-100
                          hover:text-slate-700
                          group-hover:opacity-100
                          focus:opacity-100
                        "
                      >
                        <MoreHorizontal className="h-4 w-4" />
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </>
        )}

        {/* Pagination */}
        {!isError && totalPages > 0 && (
          <div
            className="
              flex
              flex-col
              gap-3
              border-t
              border-slate-200
              px-5
              py-4
              sm:flex-row
              sm:items-center
              sm:justify-between
            "
          >
            <p className="text-xs text-slate-500">
              Page{" "}
              <span className="font-medium text-slate-700">
                {page}
              </span>{" "}
              of{" "}
              <span className="font-medium text-slate-700">
                {totalPages}
              </span>
            </p>

            <div className="flex items-center gap-2">
              <Button
                type="button"
                variant="outline"
                size="sm"
                disabled={
                  !hasPreviousPage || isLoading
                }
                onClick={handlePreviousPage}
              >
                <ChevronLeft className="h-4 w-4" />
                Previous
              </Button>

              <Button
                type="button"
                variant="outline"
                size="sm"
                disabled={
                  !hasNextPage || isLoading
                }
                onClick={handleNextPage}
              >
                Next
                <ChevronRight className="h-4 w-4" />
              </Button>
            </div>
          </div>
        )}
      </section>
    </div>
  );
}

/* ─────────────────────────────────────────
   Status Badge
   ───────────────────────────────────────── */

function ProjectStatus({
  status,
}: {
  status: string;
}) {
  const normalized = status
    .toLowerCase()
    .trim();

  const isActive =
    normalized === "active";

  return (
    <span
      className={
        isActive
          ? `
            inline-flex
            items-center
            gap-1.5
            rounded-full
            bg-emerald-50
            px-2.5
            py-1
            text-xs
            font-medium
            text-emerald-700
          `
          : `
            inline-flex
            items-center
            gap-1.5
            rounded-full
            bg-slate-100
            px-2.5
            py-1
            text-xs
            font-medium
            text-slate-600
          `
      }
    >
      <span
        className={
          isActive
            ? "h-1.5 w-1.5 rounded-full bg-emerald-500"
            : "h-1.5 w-1.5 rounded-full bg-slate-400"
        }
      />

      {status === "0" ? "Unknown" : status}
    </span>
  );
}

/* ─────────────────────────────────────────
   Visibility Badge
   ───────────────────────────────────────── */

function ProjectVisibility({
  visibility,
}: {
  visibility: string;
}) {
  const normalized = visibility
    .toLowerCase()
    .trim();

  const label =
    normalized === "private"
      ? "Private"
      : normalized === "public"
        ? "Public"
        : visibility === "0"
          ? "Unknown"
          : visibility;

  return (
    <span className="inline-flex rounded-full bg-slate-100 px-2.5 py-1 text-xs font-medium text-slate-600">
      {label}
    </span>
  );
}