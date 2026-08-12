import {
  CheckCircle2,
  FolderKanban,
  UserPlus,
} from "lucide-react";

import type { RecentActivityItem } from "../types/dashboard";

interface RecentActivityProps {
  activities: RecentActivityItem[];
}

function getActivityIcon(type: string) {
  switch (type) {
    case "task":
      return CheckCircle2;

    case "project":
      return FolderKanban;

    case "member":
      return UserPlus;

    default:
      return CheckCircle2;
  }
}

function getActivityIconClass(type: string) {
  switch (type) {
    case "task":
      return "bg-emerald-50 text-emerald-600";

    case "project":
      return "bg-slate-50 text-slate-500";

    case "member":
      return "bg-sky-50 text-sky-600";

    default:
      return "bg-slate-50 text-slate-500";
  }
}

export function RecentActivity({
  activities,
}: RecentActivityProps) {
  return (
    <section
      className="
        overflow-hidden
        rounded-2xl
        border
        border-slate-200/80
        bg-[#ffffff]
        shadow-[0_1px_2px_rgba(15,23,42,0.03)]
      "
    >
      {/* Header */}
      <div
        className="
          border-b
          border-slate-100
          px-5
          py-4
        "
      >
        <h2
          className="
            text-sm
            font-semibold
            tracking-tight
            text-slate-900
          "
        >
          Recent Activity
        </h2>

        <p
          className="
            mt-1
            text-xs
            text-slate-400
          "
        >
          Latest activity from your workspace.
        </p>
      </div>

      {activities.length === 0 ? (
        <div className="px-5 py-14 text-center">
          <div
            className="
              mx-auto
              flex
              h-11
              w-11
              items-center
              justify-center
              rounded-xl
              bg-slate-50
              ring-1
              ring-slate-100
            "
          >
            <CheckCircle2
              className="h-5 w-5 text-slate-400"
            />
          </div>

          <p
            className="
              mt-3
              text-sm
              font-medium
              text-slate-700
            "
          >
            No recent activity
          </p>

          <p
            className="
              mt-1
              text-xs
              text-slate-400
            "
          >
            Workspace activity will appear here.
          </p>
        </div>
      ) : (
        <div className="divide-y divide-slate-100">
          {activities.map((activity) => {
            const Icon = getActivityIcon(activity.type);

            return (
              <div
                key={activity.id}
                className="
                  group
                  flex
                  gap-3
                  px-5
                  py-4
                  transition-colors
                  hover:bg-slate-50/70
                "
              >
                {/* Icon */}
                <div
                  className={`
                    flex
                    h-9
                    w-9
                    shrink-0
                    items-center
                    justify-center
                    rounded-xl
                    ${getActivityIconClass(activity.type)}
                  `}
                >
                  <Icon className="h-4 w-4" />
                </div>

                {/* Content */}
                <div className="min-w-0 flex-1">
                  <div
                    className="
                      flex
                      items-start
                      justify-between
                      gap-3
                    "
                  >
                    <p
                      className="
                        text-sm
                        font-medium
                        text-slate-800
                      "
                    >
                      {activity.title}
                    </p>

                    <span
                      className="
                        shrink-0
                        text-[10px]
                        font-medium
                        text-slate-400
                      "
                    >
                      {activity.time}
                    </span>
                  </div>

                  <p
                    className="
                      mt-1
                      text-xs
                      leading-5
                      text-slate-400
                    "
                  >
                    {activity.description}
                  </p>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </section>
  );
}