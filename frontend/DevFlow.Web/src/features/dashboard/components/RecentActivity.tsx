import {
  CheckCircle2,
  FolderKanban,
  UserPlus,
} from "lucide-react";

import type { RecentActivityItem } from "../types/dashboard";

interface RecentActivityProps {
  activities: RecentActivityItem[];
}

export function RecentActivity({
  activities,
}: RecentActivityProps) {
  return (
    <section className="rounded-xl border border-slate-200 bg-white shadow-sm">
      <div className="border-b border-slate-100 px-6 py-5">
        <h2 className="text-sm font-semibold text-slate-900">
          Recent Activity
        </h2>

        <p className="mt-1 text-xs text-slate-500">
          Latest activity across your workspace.
        </p>
      </div>

      <div className="divide-y divide-slate-100">
        {activities.map((activity) => {
          const Icon =
            activity.type === "project"
              ? FolderKanban
              : activity.type === "member"
                ? UserPlus
                : CheckCircle2;

          return (
            <div
              key={activity.id}
              className="flex gap-3 px-6 py-4"
            >
              <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-slate-100">
                <Icon className="h-4 w-4 text-slate-600" />
              </div>

              <div className="min-w-0 flex-1">
                <p className="text-sm font-medium text-slate-800">
                  {activity.title}
                </p>

                <p className="mt-0.5 text-xs text-slate-500">
                  {activity.description}
                </p>

                <p className="mt-1 text-xs text-slate-400">
                  {activity.time}
                </p>
              </div>
            </div>
          );
        })}
      </div>
    </section>
  );
}