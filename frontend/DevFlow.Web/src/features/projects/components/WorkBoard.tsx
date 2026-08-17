import { useState } from "react";
import {
  AlertCircle,
  CheckCircle2,
  CircleDot,
  ClipboardCheck,
  FlaskConical,
  XCircle,
} from "lucide-react";

import { Button } from "@/components/ui/button";

import {
  WorkItemStatus,
  type WorkItem,
} from "../api/project-resources-api";

type Member = {
  userId: string;
  memberName: string;
  role: string;
};

type Column = {
  status: WorkItemStatus;
  title: string;
  icon: React.ReactNode;
  iconClass: string;
};

const columns: Column[] = [
  {
    status: WorkItemStatus.Todo,
    title: "To do",
    icon: <CircleDot className="h-4 w-4" />,
    iconClass: "text-slate-500",
  },
  {
    status: WorkItemStatus.InProgress,
    title: "In progress",
    icon: <ClipboardCheck className="h-4 w-4" />,
    iconClass: "text-blue-600",
  },
  {
    status: WorkItemStatus.InReview,
    title: "In review",
    icon: <AlertCircle className="h-4 w-4" />,
    iconClass: "text-amber-600",
  },
  {
    status: WorkItemStatus.Testing,
    title: "Testing",
    icon: <FlaskConical className="h-4 w-4" />,
    iconClass: "text-violet-600",
  },
  {
    status: WorkItemStatus.Done,
    title: "Done",
    icon: <CheckCircle2 className="h-4 w-4" />,
    iconClass: "text-emerald-600",
  },
  {
    status: WorkItemStatus.Cancelled,
    title: "Cancelled",
    icon: <XCircle className="h-4 w-4" />,
    iconClass: "text-red-600",
  },
];

function statusNumber(value: string | number) {
  if (typeof value === "number") {
    return value;
  }

  const normalized = value.replace(/\s/g, "").toLowerCase();

  const map: Record<string, WorkItemStatus> = {
    todo: WorkItemStatus.Todo,
    inprogress: WorkItemStatus.InProgress,
    inreview: WorkItemStatus.InReview,
    testing: WorkItemStatus.Testing,
    done: WorkItemStatus.Done,
    cancelled: WorkItemStatus.Cancelled,
  };

  return map[normalized] ?? WorkItemStatus.Todo;
}

function priorityClass(priority: string | number) {
  const numeric =
    typeof priority === "number" ? priority : 3;

  if (numeric >= 5) {
    return "bg-red-100 text-red-700";
  }

  if (numeric === 4) {
    return "bg-orange-100 text-orange-700";
  }

  if (numeric === 3) {
    return "bg-blue-100 text-blue-700";
  }

  return "bg-slate-100 text-slate-600";
}

function priorityLabel(priority: string | number) {
  if (typeof priority === "string") {
    return priority;
  }

  return (
    {
      1: "Lowest",
      2: "Low",
      3: "Medium",
      4: "High",
      5: "Highest",
    }[priority] ?? "Medium"
  );
}

export function WorkBoard({
  workItems,
  members,
  onSelect,
  onMove,
  canMove,
}: {
  workItems: WorkItem[];
  members: Member[];
  onSelect: (workItem: WorkItem) => void;
    onMove: (
    workItem: WorkItem,
    status: WorkItemStatus,
    ) => Promise<void>;
  canMove: boolean;
}) {

    const [draggedWorkItemId, setDraggedWorkItemId] =
        useState<string | null>(null);

    const [dropTarget, setDropTarget] =
        useState<WorkItemStatus | null>(null);

  return (
  <div className="overflow-x-auto p-5">
    <div className="grid min-w-[1320px] grid-cols-6 gap-4">
      {columns.map((column) => {
        const items = workItems.filter(
          (workItem) =>
            statusNumber(workItem.status) === column.status,
        );

        return (
          <div
            key={column.status}
            onDragOver={(event) => {
                if (!canMove) return;
                event.preventDefault();
                setDropTarget(column.status);
            }}
            onDragLeave={() => setDropTarget(null)}
            onDrop={async (event) => {
                if (!canMove) return;
                event.preventDefault();

                const workItem = workItems.find(
                (item) => item.id === draggedWorkItemId,
                );

                setDropTarget(null);
                setDraggedWorkItemId(null);

                if (
                !workItem ||
                statusNumber(workItem.status) === column.status
                ) {
                return;
                }

                await onMove(workItem, column.status);
            }}
            className={`rounded-xl p-3 transition-colors ${
                dropTarget === column.status
                ? "bg-blue-50 ring-2 ring-blue-200"
                : "bg-slate-50"
            }`}
            >

            <div className="flex items-center justify-between gap-2 px-1 pb-3">
              <div
                className={`flex items-center gap-2 text-sm font-semibold ${column.iconClass}`}
              >
                {column.icon}
                {column.title}
              </div>

              <span className="rounded-full bg-white px-2 py-0.5 text-xs font-medium text-slate-500 ring-1 ring-slate-200">
                {items.length}
              </span>
            </div>

            <div className="space-y-3">
              {items.map((workItem) => {
                const assignee = members.find(
                  (member) =>
                    member.userId === workItem.assigneeId,
                );

                return (
                  <button
                    key={workItem.id}
                    type="button"
                    draggable={canMove}
                    onDragStart={() => canMove && setDraggedWorkItemId(workItem.id)}
                    onDragEnd={() => {
                        setDraggedWorkItemId(null);
                        setDropTarget(null);
                    }}
                    onClick={() => onSelect(workItem)}
                    className="w-full rounded-xl border border-slate-200 bg-white p-3 text-left shadow-sm transition hover:-translate-y-0.5 hover:border-slate-300 hover:shadow-md focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-slate-400"
                  >
                    <div className="flex items-start justify-between gap-2">
                      <span className="text-xs font-semibold text-slate-400">
                        {workItem.key}
                      </span>

                      <span
                        className={`rounded-full px-2 py-0.5 text-[10px] font-semibold ${priorityClass(
                          workItem.priority,
                        )}`}
                      >
                        {priorityLabel(workItem.priority)}
                      </span>
                    </div>

                    <p className="mt-2 line-clamp-2 text-sm font-medium leading-5 text-slate-800">
                      {workItem.title}
                    </p>

                    <div className="mt-3 flex items-center justify-between gap-2">
                      <span className="text-xs text-slate-400">
                        {workItem.dueDate
                          ? new Intl.DateTimeFormat(undefined, {
                              month: "short",
                              day: "numeric",
                            }).format(
                              new Date(workItem.dueDate),
                            )
                          : "No due date"}
                      </span>

                      {assignee && (
                        <span
                          title={assignee.memberName}
                          className="flex h-6 w-6 items-center justify-center rounded-full bg-slate-100 text-[10px] font-semibold text-slate-600"
                        >
                          {assignee.memberName
                            .split(/\s+/)
                            .filter(Boolean)
                            .slice(0, 2)
                            .map((part) => part[0])
                            .join("")
                            .toUpperCase()}
                        </span>
                      )}
                    </div>
                  </button>
                );
              })}

              {items.length === 0 && (
                <div className="rounded-lg border border-dashed border-slate-200 px-3 py-7 text-center text-xs text-slate-400">
                  No work items
                </div>
              )}
            </div>
          </div>
        );
      })}
    </div>
  </div>
);
}
