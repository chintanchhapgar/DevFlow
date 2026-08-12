import {
  FolderKanban,
  ListTodo,
  TrendingUp,
  Users,
} from "lucide-react";

import type { DashboardStat } from "../types/dashboard";

interface DashboardStatsProps {
  stats: DashboardStat[];
}

const icons = [
  FolderKanban,
  ListTodo,
  TrendingUp,
  Users,
];

export function DashboardStats({
  stats,
}: DashboardStatsProps) {
  return (
    <section className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
      {stats.map((stat, index) => {
        const Icon = icons[index] ?? FolderKanban;

        return (
          <div
            key={stat.label}
            className="
              group
              relative
              overflow-hidden
              rounded-2xl
              border
              border-slate-200/80
              bg-[#ffffff]
              p-5
              shadow-[0_1px_2px_rgba(15,23,42,0.03)]
              transition-all
              duration-200
              hover:-translate-y-0.5
              hover:border-slate-200
              hover:shadow-[0_8px_30px_rgba(15,23,42,0.06)]
            "
          >
            {/* Subtle top accent */}
            <div
              className="
                absolute
                inset-x-0
                top-0
                h-px
                bg-[var(--devflow-primary)]
                opacity-0
                transition-opacity
                duration-200
                group-hover:opacity-60
              "
            />

            <div className="flex items-start justify-between">
              <div
                className="
                  flex
                  h-10
                  w-10
                  items-center
                  justify-center
                  rounded-xl
                  bg-slate-50
                  ring-1
                  ring-slate-100
                  transition-colors
                  group-hover:bg-[var(--devflow-primary)]/5
                "
              >
                <Icon
                  className="
                    h-[18px]
                    w-[18px]
                    text-slate-500
                    transition-colors
                    group-hover:text-[var(--devflow-primary)]
                  "
                />
              </div>

              {stat.trend && (
                <span
                  className="
                    inline-flex
                    items-center
                    rounded-full
                    bg-emerald-50
                    px-2
                    py-1
                    text-[11px]
                    font-medium
                    text-emerald-600
                  "
                >
                  {stat.trend}
                </span>
              )}
            </div>

            <div className="mt-5">
              <p
                className="
                  text-xs
                  font-medium
                  tracking-wide
                  text-slate-500
                "
              >
                {stat.label}
              </p>

              <p
                className="
                  mt-1
                  text-[28px]
                  font-semibold
                  leading-none
                  tracking-tight
                  text-slate-900
                "
              >
                {stat.value}
              </p>

              <p
                className="
                  mt-2
                  text-xs
                  leading-5
                  text-slate-400
                "
              >
                {stat.description}
              </p>
            </div>
          </div>
        );
      })}
    </section>
  );
}