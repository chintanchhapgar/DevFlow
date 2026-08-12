import {
  CheckCircle2,
  FolderKanban,
  ListTodo,
  Users,
} from "lucide-react";

import type { DashboardStat } from "../types/dashboard";

const icons = [
  FolderKanban,
  ListTodo,
  CheckCircle2,
  Users,
];

interface DashboardStatsProps {
  stats: DashboardStat[];
}

export function DashboardStats({
  stats,
}: DashboardStatsProps) {
  return (
    <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
      {stats.map((stat, index) => {
        const Icon = icons[index] ?? FolderKanban;

        return (
          <div
            key={stat.label}
            className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm"
          >
            <div className="flex items-center justify-between">
              <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-blue-50">
                <Icon className="h-5 w-5 text-blue-600" />
              </div>

              {stat.trend && (
                <span className="text-xs font-medium text-emerald-600">
                  {stat.trend}
                </span>
              )}
            </div>

            <p className="mt-4 text-sm text-slate-500">
              {stat.label}
            </p>

            <p className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
              {stat.value}
            </p>

            <p className="mt-1 text-xs text-slate-400">
              {stat.description}
            </p>
          </div>
        );
      })}
    </div>
  );
}