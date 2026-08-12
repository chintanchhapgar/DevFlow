import {
  ArrowRight,
  FolderKanban,
} from "lucide-react";
import { Link } from "react-router-dom";

import type { RecentProject } from "../types/dashboard";

interface RecentProjectsProps {
  projects: RecentProject[];
}

function getStatusClass(status: string) {
  switch (status.toLowerCase()) {
    case "active":
      return "bg-emerald-50 text-emerald-600";

    case "planning":
      return "bg-amber-50 text-amber-600";

    case "completed":
      return "bg-slate-100 text-slate-600";

    default:
      return "bg-slate-100 text-slate-500";
  }
}

export function RecentProjects({
  projects,
}: RecentProjectsProps) {
  return (
    <section
      className="
        overflow-hidden
        rounded-2xl
        border border-slate-200/80
        bg-white
        shadow-[0_1px_2px_rgba(15,23,42,0.03)]
      "
    >
      {/* Header */}
      <div
        className="
          flex
          items-center
          justify-between
          border-b
          border-slate-100
          px-5
          py-4
        "
      >
        <div>
          <h2
            className="
              text-sm
              font-semibold
              tracking-tight
              text-slate-900
            "
          >
            Recent Projects
          </h2>

          <p
            className="
              mt-1
              text-xs
              text-slate-400
            "
          >
            Projects you&apos;ve recently worked with.
          </p>
        </div>

        <Link
          to="/projects"
          className="
            inline-flex
            items-center
            gap-1.5
            rounded-lg
            px-2.5
            py-1.5
            text-xs
            font-medium
            text-slate-500
            transition-colors
            hover:bg-slate-50
            hover:text-[var(--devflow-primary)]
          "
        >
          View all
          <ArrowRight className="h-3.5 w-3.5" />
        </Link>
      </div>

      {/* Empty State */}
      {projects.length === 0 ? (
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
            <FolderKanban className="h-5 w-5 text-slate-400" />
          </div>

          <p
            className="
              mt-3
              text-sm
              font-medium
              text-slate-700
            "
          >
            No projects found
          </p>

          <p
            className="
              mt-1
              text-xs
              text-slate-400
            "
          >
            Create a project to get started.
          </p>
        </div>
      ) : (
        <div className="divide-y divide-slate-100">
          {projects.map((project) => {
            const progress = Math.min(
              Math.max(Number(project.progress) || 0, 0),
              100,
            );

            return (
              <Link
                key={project.id}
                to="/projects"
                className="
                  group
                  block
                  px-5
                  py-4
                  transition-colors
                  hover:bg-slate-50/70
                "
              >
                <div className="flex items-start gap-4">
                  {/* Project Icon */}
                  <div
                    className="
                      flex
                      h-9
                      w-9
                      shrink-0
                      items-center
                      justify-center
                      rounded-xl
                      bg-slate-50
                      ring-1
                      ring-slate-100
                      transition-colors
                      group-hover:bg-slate-100
                    "
                  >
                    <FolderKanban
                      className="
                        h-4
                        w-4
                        text-slate-500
                        transition-colors
                        group-hover:text-[var(--devflow-primary)]
                      "
                    />
                  </div>

                  {/* Project Content */}
                  <div className="min-w-0 flex-1">
                    {/* Name + Status */}
                    <div className="flex items-center gap-2">
                      <h3
                        className="
                          truncate
                          text-sm
                          font-medium
                          text-slate-800
                          transition-colors
                          group-hover:text-[var(--devflow-primary)]
                        "
                      >
                        {project.name}
                      </h3>

                      <span
                        className={`
                          shrink-0
                          rounded-full
                          px-2
                          py-0.5
                          text-[10px]
                          font-medium
                          ${getStatusClass(project.status)}
                        `}
                      >
                        {project.status}
                      </span>
                    </div>

                    {/* Description */}
                    <p
                      className="
                        mt-1
                        truncate
                        text-xs
                        text-slate-400
                      "
                    >
                      {project.description}
                    </p>

                    {/* Progress */}
                    <div className="mt-4">
                      {/* Progress Header */}
                      <div
                        className="
                          mb-1.5
                          flex
                          items-center
                          justify-between
                        "
                      >
                        <span
                          className="
                            text-[10px]
                            font-medium
                            uppercase
                            tracking-wide
                            text-slate-400
                          "
                        >
                          Progress
                        </span>

                        <span
                          className="
                            text-[11px]
                            font-semibold
                            tabular-nums
                            text-slate-600
                          "
                        >
                          {progress}%
                        </span>
                      </div>

                      {/* Progress Track */}
                      <div
                        className="
                          relative
                          h-2
                          w-full
                          overflow-hidden
                          rounded-full
                          bg-slate-100
                        "
                      >
                        {/* Progress Fill */}
                        <div
                          className="
                            absolute
                            inset-y-0
                            left-0
                            rounded-full
                            transition-all
                            duration-500
                            ease-out
                          "
                          style={{
                            width: `${progress}%`,
                            backgroundColor:
                              "var(--devflow-primary)",
                          }}
                        />
                      </div>
                    </div>
                  </div>

                  {/* Arrow */}
                  <ArrowRight
                    className="
                      mt-1
                      h-4
                      w-4
                      shrink-0
                      text-slate-300
                      transition-all
                      duration-200
                      group-hover:translate-x-0.5
                      group-hover:text-[var(--devflow-primary)]
                    "
                  />
                </div>
              </Link>
            );
          })}
        </div>
      )}
    </section>
  );
}