import {
  useMemo,
  useState,
} from "react";
import {
  AlertCircle,
  CalendarDays,
  ClipboardList,
  LayoutList,
  PanelsTopLeft,
  Search,
} from "lucide-react";
import { Link } from "react-router-dom";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

import {
  WorkItemPriority,
  WorkItemStatus,
} from "../api/project-resources-api";
import { WorkBoard } from "../components/WorkBoard";
import { WorkItemDetailDialog } from "../components/WorkItemDetailDialog";
import { useProject } from "../hooks/use-project";
import {
  useChangeWorkItemStatus,
} from "../hooks/use-project-resources";
import {
  type MyWorkItem,
  useMyWork,
} from "../hooks/use-my-work";

type View = "board" | "list";

function statusValue(value: string | number) {
  if (typeof value === "number") {
    return value;
  }

  const statuses: Record<string, WorkItemStatus> = {
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

  return statuses[value.toLowerCase()] ?? WorkItemStatus.Todo;
}

function priorityValue(value: string | number) {
  if (typeof value === "number") {
    return value;
  }

  const priorities: Record<string, WorkItemPriority> = {
    lowest: WorkItemPriority.Lowest,
    low: WorkItemPriority.Low,
    medium: WorkItemPriority.Medium,
    high: WorkItemPriority.High,
    highest: WorkItemPriority.Highest,
  };

  return (
    priorities[value.toLowerCase()] ??
    WorkItemPriority.Medium
  );
}

function statusLabel(value: string | number) {
  const labels: Record<number, string> = {
    1: "To do",
    2: "In progress",
    3: "In review",
    4: "Testing",
    5: "Done",
    6: "Cancelled",
  };

  return labels[statusValue(value)] ?? "To do";
}

export function WorkPage() {
  const myWorkQuery = useMyWork();
  const changeStatus = useChangeWorkItemStatus();

  const [view, setView] = useState<View>("board");
  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState<
    "all" | WorkItemStatus
  >("all");
  const [priorityFilter, setPriorityFilter] = useState<
    "all" | WorkItemPriority
  >("all");
  const [selectedWorkItemId, setSelectedWorkItemId] =
    useState<string | null>(null);

  const selectedWorkItem =
    myWorkQuery.items.find(
      (item) => item.id === selectedWorkItemId,
    ) ?? null;

  const selectedProjectQuery = useProject(
    selectedWorkItem?.projectId,
  );

  const filteredItems = useMemo(() => {
    const searchTerm = search.trim().toLowerCase();

    return myWorkQuery.items.filter((item) => {
      const matchesSearch =
        !searchTerm ||
        item.title.toLowerCase().includes(searchTerm) ||
        item.key.toLowerCase().includes(searchTerm) ||
        item.projectName
          .toLowerCase()
          .includes(searchTerm);

      const matchesStatus =
        statusFilter === "all" ||
        statusValue(item.status) === statusFilter;

      const matchesPriority =
        priorityFilter === "all" ||
        priorityValue(item.priority) === priorityFilter;

      return (
        matchesSearch &&
        matchesStatus &&
        matchesPriority
      );
    });
  }, [
    myWorkQuery.items,
    priorityFilter,
    search,
    statusFilter,
  ]);

  async function moveWorkItem(
    item: MyWorkItem,
    status: WorkItemStatus,
  ) {
    await changeStatus.mutateAsync({
      projectId: item.projectId,
      workItemId: item.id,
      status,
    });

    await myWorkQuery.refetch();
  }

  return (
    <div className="mx-auto w-full max-w-7xl space-y-6">
      <div>
        <p className="text-sm font-medium text-[var(--devflow-primary)]">
          Workspace
        </p>

        <h1 className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
          My work
        </h1>

        <p className="mt-1.5 text-sm text-slate-500">
          Track work across every project you can access.
        </p>
      </div>

      <section className="rounded-2xl border border-slate-200 bg-white p-4 shadow-sm">
        <div className="flex flex-col gap-3 lg:flex-row lg:items-center">
          <div className="relative flex-1">
            <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />

            <Input
              type="search"
              value={search}
              placeholder="Search work, project, or key..."
              className="pl-9"
              onChange={(event) =>
                setSearch(event.target.value)
              }
            />
          </div>

          <select
            value={statusFilter}
            onChange={(event) =>
              setStatusFilter(
                event.target.value === "all"
                  ? "all"
                  : (Number(
                      event.target.value,
                    ) as WorkItemStatus),
              )
            }
            className="h-10 rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
          >
            <option value="all">All statuses</option>
            <option value={WorkItemStatus.Todo}>To do</option>
            <option value={WorkItemStatus.InProgress}>
              In progress
            </option>
            <option value={WorkItemStatus.InReview}>
              In review
            </option>
            <option value={WorkItemStatus.Testing}>
              Testing
            </option>
            <option value={WorkItemStatus.Done}>Done</option>
            <option value={WorkItemStatus.Cancelled}>
              Cancelled
            </option>
          </select>

          <select
            value={priorityFilter}
            onChange={(event) =>
              setPriorityFilter(
                event.target.value === "all"
                  ? "all"
                  : (Number(
                      event.target.value,
                    ) as WorkItemPriority),
              )
            }
            className="h-10 rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700 outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
          >
            <option value="all">All priorities</option>
            <option value={WorkItemPriority.Lowest}>
              Lowest
            </option>
            <option value={WorkItemPriority.Low}>Low</option>
            <option value={WorkItemPriority.Medium}>
              Medium
            </option>
            <option value={WorkItemPriority.High}>High</option>
            <option value={WorkItemPriority.Highest}>
              Highest
            </option>
          </select>

          <div className="flex rounded-lg border border-slate-200 bg-slate-50 p-1">
            <button
              type="button"
              aria-label="Board view"
              title="Board view"
              onClick={() => setView("board")}
              className={`flex h-8 w-8 items-center justify-center rounded-md ${
                view === "board"
                  ? "bg-white text-slate-900 shadow-sm"
                  : "text-slate-400 hover:text-slate-700"
              }`}
            >
              <PanelsTopLeft className="h-4 w-4" />
            </button>

            <button
              type="button"
              aria-label="List view"
              title="List view"
              onClick={() => setView("list")}
              className={`flex h-8 w-8 items-center justify-center rounded-md ${
                view === "list"
                  ? "bg-white text-slate-900 shadow-sm"
                  : "text-slate-400 hover:text-slate-700"
              }`}
            >
              <LayoutList className="h-4 w-4" />
            </button>
          </div>
        </div>
      </section>

      {myWorkQuery.isLoading && (
        <div className="space-y-3">
          {[0, 1, 2, 3, 4].map((index) => (
            <div
              key={index}
              className="h-20 animate-pulse rounded-xl bg-slate-100"
            />
          ))}
        </div>
      )}

      {myWorkQuery.isError && (
        <section className="rounded-2xl border border-red-200 bg-red-50 p-5">
          <div className="flex items-start gap-3">
            <AlertCircle className="mt-0.5 h-5 w-5 text-red-600" />

            <div>
              <p className="font-medium text-red-800">
                Unable to load your work.
              </p>

              <p className="mt-1 text-sm text-red-700">
                Please try again.
              </p>

              <Button
                type="button"
                variant="outline"
                size="sm"
                className="mt-4"
                onClick={() => myWorkQuery.refetch()}
              >
                Try again
              </Button>
            </div>
          </div>
        </section>
      )}

      {!myWorkQuery.isLoading &&
        !myWorkQuery.isError &&
        filteredItems.length === 0 && (
          <section className="flex min-h-80 flex-col items-center justify-center rounded-2xl border border-slate-200 bg-white px-5 text-center shadow-sm">
            <ClipboardList className="h-8 w-8 text-slate-400" />

            <h2 className="mt-3 text-base font-semibold text-slate-900">
              No work items found
            </h2>

            <p className="mt-1 text-sm text-slate-500">
              {search ||
              statusFilter !== "all" ||
              priorityFilter !== "all"
                ? "Try changing or clearing your filters."
                : "Work assigned across your projects will appear here."}
            </p>

            {(search ||
              statusFilter !== "all" ||
              priorityFilter !== "all") && (
              <Button
                type="button"
                variant="outline"
                size="sm"
                className="mt-4"
                onClick={() => {
                  setSearch("");
                  setStatusFilter("all");
                  setPriorityFilter("all");
                }}
              >
                Clear filters
              </Button>
            )}
          </section>
        )}

      {!myWorkQuery.isLoading &&
        !myWorkQuery.isError &&
        filteredItems.length > 0 &&
        view === "board" && (
          <WorkBoard
            workItems={filteredItems}
            members={selectedProjectQuery.data?.members ?? []}
            onSelect={(item) => setSelectedWorkItemId(item.id)}
            onMove={moveWorkItem}
          />
        )}

      {!myWorkQuery.isLoading &&
        !myWorkQuery.isError &&
        filteredItems.length > 0 &&
        view === "list" && (
          <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
            <div className="divide-y divide-slate-100">
              {filteredItems.map((item) => (
                <button
                  key={item.id}
                  type="button"
                  onClick={() => setSelectedWorkItemId(item.id)}
                  className="flex w-full items-center gap-3 px-5 py-4 text-left transition-colors hover:bg-slate-50"
                >
                  <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-slate-100 text-slate-500">
                    <ClipboardList className="h-4 w-4" />
                  </div>

                  <div className="min-w-0 flex-1">
                    <p className="truncate text-sm font-medium text-slate-900">
                      {item.title}
                    </p>

                    <p className="mt-1 text-xs text-slate-500">
                      {item.key} · {statusLabel(item.status)}
                      {" · "}
                      <span className="font-medium">
                        {item.projectName}
                      </span>
                    </p>
                  </div>

                  <div className="hidden text-right sm:block">
                    <p className="text-xs font-medium text-slate-600">
                      {item.dueDate
                        ? `Due ${new Intl.DateTimeFormat(
                            undefined,
                            { dateStyle: "medium" },
                          ).format(new Date(item.dueDate))}`
                        : "No due date"}
                    </p>

                    <p className="mt-1 text-[11px] text-slate-400">
                      {priorityValue(item.priority) ===
                      WorkItemPriority.Highest
                        ? "Highest priority"
                        : "Work item"}
                    </p>
                  </div>
                </button>
              ))}
            </div>
          </section>
        )}

      {selectedWorkItem &&
        selectedProjectQuery.data && (
          <WorkItemDetailDialog
            open
            onOpenChange={(open) => {
              if (!open) {
                setSelectedWorkItemId(null);
                void myWorkQuery.refetch();
              }
            }}
            projectId={selectedWorkItem.projectId}
            workItem={selectedWorkItem}
            members={selectedProjectQuery.data.members ?? []}
          />
        )}
    </div>
  );
}