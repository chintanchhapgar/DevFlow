import { ArrowRight, FolderKanban } from "lucide-react";
import { Link } from "react-router-dom";

import type { RecentProject } from "../types/dashboard";

interface RecentProjectsProps {
  projects: RecentProject[];
}

export function RecentProjects({
  projects,
}: RecentProjectsProps) {
  return (
    <section className="rounded-xl border border-slate-200 bg-white shadow-sm">
      <div className="flex items-center justify-between border-b border-slate-100 px-6 py-5">
        <div>
          <h2 className="text-sm font-semibold text-slate-900">
            Recent Projects
          </h2>

          <p className="mt-1 text-xs text-slate-500">
            Overview of your latest projects.
          </p>
        </div>

        <Link
          to="/projects"
          className="inline-flex items-center gap-1 text-xs font-medium text-blue-600 hover:text-blue-700"
        >
          View all
          <ArrowRight className="h-3.5 w-3.5" />
        </Link>
      </div>

      <div className="divide-y divide-slate-100">
        {projects.map((project) => (
          <div
            key={project.id}
            className="p-5 transition hover:bg-slate-50"
          >
            <div className="flex items-start justify-between gap-4">
              <div className="flex min-w-0 gap-3">
                <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-slate-100">
                  <FolderKanban className="h-4 w-4 text-slate-600" />
                </div>

                <div className="min-w-0">
                  <h3 className="truncate text-sm font-medium text-slate-900">
                    {project.name}
                  </h3>

                  <p className="mt-1 truncate text-xs text-slate-500">
                    {project.description}
                  </p>
                </div>
              </div>

              <span
                className={[
                  "shrink-0 rounded-full px-2.5 py-1 text-xs font-medium",
                  project.status === "Active" &&
                    "bg-blue-50 text-blue-700",
                  project.status === "Planning" &&
                    "bg-amber-50 text-amber-700",
                  project.status === "Completed" &&
                    "bg-emerald-50 text-emerald-700",
                ]
                  .filter(Boolean)
                  .join(" ")}
              >
                {project.status}
              </span>
            </div>

            <div className="mt-4">
              <div className="mb-1.5 flex items-center justify-between">
                <span className="text-xs text-slate-500">
                  Progress
                </span>

                <span className="text-xs font-medium text-slate-700">
                  {project.progress}%
                </span>
              </div>

              <div className="h-1.5 overflow-hidden rounded-full bg-slate-100">
                <div
                  className="h-full rounded-full bg-blue-600 transition-all"
                  style={{
                    width: `${project.progress}%`,
                  }}
                />
              </div>
            </div>
          </div>
        ))}
      </div>
    </section>
  );
}