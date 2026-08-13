import { useEffect, useState } from "react";
import type { ReactNode } from "react";
import {
  ArrowLeft,
  CalendarDays,
  Check,
  FolderKanban,
  MoreHorizontal,
  Pencil,
  Shield,
  Users,
} from "lucide-react";
import { Link, useNavigate, useParams } from "react-router-dom";

import { Button } from "@/components/ui/button";
import { projectApiClient } from "@/lib/api/project-api-client";

/* -------------------------------------------------------------------------- */
/* Types                                                                      */
/* -------------------------------------------------------------------------- */

interface ProjectDetail {
  projectId: string;
  key: string;
  name: string;
  description: string | null;
  status: string;
  visibility: string;
  ownerId: string;
  ownerName: string;
  members: ProjectMemberResponse[];
}

interface ProjectMemberResponse {
  userId: string;
  role: string;
  memberName: string;
  joinedOnUtc: string;
}

/* -------------------------------------------------------------------------- */
/* Page                                                                       */
/* -------------------------------------------------------------------------- */

export function ProjectDetailPage() {
  const { projectId } = useParams<{
    projectId: string;
  }>();

  const navigate = useNavigate();

  const [project, setProject] =
    useState<ProjectDetail | null>(null);

  const [isLoading, setIsLoading] =
    useState(true);

  const [isError, setIsError] =
    useState(false);

  /* ------------------------------------------------------------------------ */
  /* Load project                                                             */
  /* ------------------------------------------------------------------------ */

  useEffect(() => {
    if (!projectId) {
      setProject(null);
      setIsLoading(false);
      setIsError(true);
      return;
    }

    const currentProjectId = projectId;

    let cancelled = false;

    async function loadProject() {
      setIsLoading(true);
      setIsError(false);

      try {
        const response = await projectApiClient.get(
          `/api/projects/${currentProjectId}`,
        );

        if (cancelled) {
          return;
        }

        const data = response.data?.data as
          | ProjectDetail
          | undefined;

        if (!data) {
          setProject(null);
          setIsError(true);
          return;
        }

        setProject(data);
      } catch {
        if (!cancelled) {
          setProject(null);
          setIsError(true);
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    }

    loadProject();

    return () => {
      cancelled = true;
    };
  }, [projectId]);

  /* ------------------------------------------------------------------------ */
  /* Loading                                                                  */
  /* ------------------------------------------------------------------------ */

  if (isLoading) {
    return (
      <div className="mx-auto w-full max-w-7xl">
        <div className="space-y-6">
          <div className="h-5 w-24 animate-pulse rounded bg-slate-100" />

          <div className="rounded-2xl border border-slate-200 bg-white p-6">
            <div className="flex items-start gap-4">
              <div className="h-14 w-14 animate-pulse rounded-xl bg-slate-100" />

              <div className="flex-1 space-y-3">
                <div className="h-6 w-56 animate-pulse rounded bg-slate-100" />

                <div className="h-4 w-80 animate-pulse rounded bg-slate-100" />

                <div className="flex gap-2">
                  <div className="h-6 w-16 animate-pulse rounded-full bg-slate-100" />
                  <div className="h-6 w-20 animate-pulse rounded-full bg-slate-100" />
                </div>
              </div>
            </div>
          </div>

          <div className="grid gap-6 lg:grid-cols-[minmax(0,1fr)_360px]">
            <div className="h-72 animate-pulse rounded-2xl bg-slate-100" />
            <div className="h-72 animate-pulse rounded-2xl bg-slate-100" />
          </div>
        </div>
      </div>
    );
  }

  /* ------------------------------------------------------------------------ */
  /* Error                                                                    */
  /* ------------------------------------------------------------------------ */

  if (isError || !project) {
    return (
      <div className="mx-auto flex min-h-[60vh] w-full max-w-7xl items-center justify-center">
        <div className="max-w-md text-center">
          <div className="mx-auto flex h-12 w-12 items-center justify-center rounded-xl bg-slate-50 text-slate-400 ring-1 ring-slate-200">
            <FolderKanban className="h-6 w-6" />
          </div>

          <h1 className="mt-4 text-lg font-semibold text-slate-900">
            Project not found
          </h1>

          <p className="mt-2 text-sm leading-6 text-slate-500">
            We couldn't load this project. It may have been
            removed or you may not have access to it.
          </p>

          <Button
            type="button"
            variant="outline"
            className="mt-5 border-slate-200 text-slate-700 hover:bg-slate-50"
            onClick={() => navigate("/projects")}
          >
            <ArrowLeft className="h-4 w-4" />
            Back to projects
          </Button>
        </div>
      </div>
    );
  }

  const memberCount = project.members?.length ?? 0;

  /* ------------------------------------------------------------------------ */
  /* Render                                                                   */
  /* ------------------------------------------------------------------------ */

  return (
    <div className="mx-auto w-full max-w-7xl space-y-6">
      {/* Back */}
      <Link
        to="/projects"
        className="
          inline-flex
          items-center
          gap-2
          text-sm
          font-medium
          text-slate-500
          transition-colors
          hover:text-slate-900
        "
      >
        <ArrowLeft className="h-4 w-4" />
        Projects
      </Link>

      {/* ================================================================== */}
      {/* PROJECT HEADER                                                      */}
      {/* ================================================================== */}

      <section
        className="
          overflow-hidden
          rounded-2xl
          border
          border-slate-200
          bg-white
          shadow-[0_1px_2px_rgba(15,23,42,0.04)]
        "
      >
        <div className="p-6">
          <div className="flex flex-col gap-5 lg:flex-row lg:items-start lg:justify-between">
            <div className="flex min-w-0 items-start gap-4">
              {/* Project Icon */}
              <div
                className="
                  flex
                  h-14
                  w-14
                  shrink-0
                  items-center
                  justify-center
                  rounded-xl
                  bg-slate-50
                  text-sm
                  font-semibold
                  text-slate-600
                  ring-1
                  ring-slate-200
                "
              >
                {getProjectInitials(project)}
              </div>

              {/* Project Information */}
              <div className="min-w-0">
                <div className="flex flex-wrap items-center gap-2">
                  <h1 className="text-2xl font-semibold tracking-tight text-slate-900">
                    {project.name || "Unnamed project"}
                  </h1>

                  {project.key && (
                    <span
                      className="
                        rounded-md
                        bg-slate-100
                        px-2
                        py-1
                        text-xs
                        font-semibold
                        text-slate-600
                      "
                    >
                      {project.key}
                    </span>
                  )}
                </div>

                <p className="mt-2 max-w-2xl text-sm leading-6 text-slate-500">
                  {project.description ||
                    "No project description has been added yet."}
                </p>

                {/* Owner */}
                <div className="mt-3 flex items-center gap-2 text-sm text-slate-500">
                  <span>Owned by</span>

                  <span className="font-medium text-slate-800">
                    {project.ownerName || "Unknown User"}
                  </span>
                </div>

                {/* Badges */}
                <div className="mt-4 flex flex-wrap items-center gap-2">
                  <ProjectStatus
                    status={project.status}
                  />

                  <ProjectVisibility
                    visibility={project.visibility}
                  />

                  <span
                    className="
                      inline-flex
                      items-center
                      gap-1.5
                      rounded-full
                      bg-slate-50
                      px-2.5
                      py-1
                      text-xs
                      font-medium
                      text-slate-600
                    "
                  >
                    <Users className="h-3.5 w-3.5" />

                    {memberCount}{" "}
                    {memberCount === 1
                      ? "member"
                      : "members"}
                  </span>
                </div>
              </div>
            </div>

            {/* Actions */}
            <div className="flex shrink-0 items-center gap-2">
              <Button
                type="button"
                variant="outline"
                className="
                  border-slate-200
                  text-slate-700
                  hover:bg-slate-50
                "
              >
                <Pencil className="h-4 w-4" />
                Edit
              </Button>

              <button
                type="button"
                aria-label="Project actions"
                className="
                  flex
                  h-9
                  w-9
                  items-center
                  justify-center
                  rounded-lg
                  border
                  border-slate-200
                  text-slate-400
                  transition-colors
                  hover:bg-slate-50
                  hover:text-slate-700
                "
              >
                <MoreHorizontal className="h-4 w-4" />
              </button>
            </div>
          </div>
        </div>

        {/* Tabs */}
        <div className="border-t border-slate-100 px-6">
          <nav className="flex gap-6 overflow-x-auto">
            <ProjectTab
              active
              label="Overview"
            />

            <ProjectTab
              label="Members"
              count={memberCount}
            />

            <ProjectTab label="Work" />

            <ProjectTab label="Attachments" />
          </nav>
        </div>
      </section>

      {/* ================================================================== */}
      {/* CONTENT                                                             */}
      {/* ================================================================== */}

      <div className="grid gap-6 lg:grid-cols-[minmax(0,1fr)_360px]">
        {/* Project Overview */}
        <section
          className="
            overflow-hidden
            rounded-2xl
            border
            border-slate-200
            bg-white
            shadow-[0_1px_2px_rgba(15,23,42,0.04)]
          "
        >
          <div className="border-b border-slate-100 px-6 py-5">
            <h2 className="text-base font-semibold text-slate-900">
              Project overview
            </h2>

            <p className="mt-1 text-sm text-slate-500">
              Basic information about this project.
            </p>
          </div>

          <div className="grid gap-px bg-slate-100 sm:grid-cols-2">
            <InfoItem
              label="Project key"
              value={project.key || "—"}
            />

            <InfoItem
              label="Status"
              value={project.status || "—"}
            />

            <InfoItem
              label="Visibility"
              value={project.visibility || "—"}
            />

            <InfoItem
              label="Owner"
              value={project.ownerName || "Unknown User"}
            />

            <InfoItem
              label="Owner ID"
              value={project.ownerId}
              mono
            />

            <InfoItem
              label="Project ID"
              value={project.projectId}
              mono
            />
          </div>
        </section>

        {/* Members */}
        <section
          className="
            overflow-hidden
            rounded-2xl
            border
            border-slate-200
            bg-white
            shadow-[0_1px_2px_rgba(15,23,42,0.04)]
          "
        >
          <div className="flex items-center justify-between border-b border-slate-100 px-5 py-4">
            <div>
              <h2 className="text-base font-semibold text-slate-900">
                Members
              </h2>

              <p className="mt-0.5 text-xs text-slate-500">
                {memberCount}{" "}
                {memberCount === 1
                  ? "member"
                  : "members"}
              </p>
            </div>

            <button
              type="button"
              className="
                text-xs
                font-medium
                text-slate-500
                transition-colors
                hover:text-slate-900
              "
            >
              Manage
            </button>
          </div>

          {/* Members Empty */}
          {memberCount === 0 && (
            <div className="px-5 py-10 text-center">
              <div className="mx-auto flex h-10 w-10 items-center justify-center rounded-lg bg-slate-50 text-slate-400">
                <Users className="h-5 w-5" />
              </div>

              <p className="mt-3 text-sm font-medium text-slate-900">
                No members
              </p>

              <p className="mt-1 text-xs text-slate-500">
                This project doesn't have any members yet.
              </p>
            </div>
          )}

          {/* Members */}
          {memberCount > 0 && (
            <div className="divide-y divide-slate-100">
              {project.members.map((member) => (
                <MemberRow
                  key={member.userId}
                  member={member}
                  ownerId={project.ownerId}
                />
              ))}
            </div>
          )}
        </section>
      </div>

      {/* ================================================================== */}
      {/* QUICK INFO                                                          */}
      {/* ================================================================== */}

      <section
        className="
          rounded-2xl
          border
          border-slate-200
          bg-white
          shadow-[0_1px_2px_rgba(15,23,42,0.04)]
        "
      >
        <div className="grid divide-y divide-slate-100 sm:grid-cols-3 sm:divide-x sm:divide-y-0">
          <QuickInfo
            icon={<Users className="h-4 w-4" />}
            label="Team"
            value={`${memberCount} ${
              memberCount === 1
                ? "member"
                : "members"
            }`}
          />

          <QuickInfo
            icon={<Shield className="h-4 w-4" />}
            label="Visibility"
            value={project.visibility || "Unknown"}
          />

          <QuickInfo
            icon={<CalendarDays className="h-4 w-4" />}
            label="Owner"
            value={project.ownerName || "Unknown User"}
          />
        </div>
      </section>
    </div>
  );
}

/* -------------------------------------------------------------------------- */
/* Project Tab                                                                */
/* -------------------------------------------------------------------------- */

function ProjectTab({
  label,
  count,
  active = false,
}: {
  label: string;
  count?: number;
  active?: boolean;
}) {
  return (
    <button
      type="button"
      className={
        active
          ? `
            relative
            whitespace-nowrap
            py-3
            text-sm
            font-medium
            text-slate-900
            after:absolute
            after:inset-x-0
            after:bottom-0
            after:h-0.5
            after:bg-slate-900
          `
          : `
            whitespace-nowrap
            py-3
            text-sm
            font-medium
            text-slate-400
            transition-colors
            hover:text-slate-700
          `
      }
    >
      {label}

      {typeof count === "number" && (
        <span className="ml-1.5 text-xs text-slate-400">
          {count}
        </span>
      )}
    </button>
  );
}

/* -------------------------------------------------------------------------- */
/* Info Item                                                                  */
/* -------------------------------------------------------------------------- */

function InfoItem({
  label,
  value,
  mono = false,
}: {
  label: string;
  value: string;
  mono?: boolean;
}) {
  return (
    <div className="bg-white px-6 py-4">
      <p className="text-[11px] font-semibold uppercase tracking-wider text-slate-400">
        {label}
      </p>

      <p
        className={
          mono
            ? "mt-1 truncate font-mono text-xs text-slate-600"
            : "mt-1 truncate text-sm font-medium text-slate-800"
        }
      >
        {value}
      </p>
    </div>
  );
}

/* -------------------------------------------------------------------------- */
/* Member Row                                                                 */
/* -------------------------------------------------------------------------- */

function MemberRow({
  member,
  ownerId,
}: {
  member: ProjectMemberResponse;
  ownerId: string;
}) {
  const isOwner =
    member.userId === ownerId;

  const memberName =
    member.memberName?.trim() || "Unknown User";

  return (
    <div className="flex items-center gap-3 px-5 py-3.5">
      {/* Avatar */}
      <div
        className="
          flex
          h-9
          w-9
          shrink-0
          items-center
          justify-center
          rounded-full
          bg-slate-100
          text-xs
          font-semibold
          text-slate-600
        "
      >
        {getMemberInitials(memberName)}
      </div>

      {/* User */}
      <div className="min-w-0 flex-1">
        <p className="truncate text-sm font-medium text-slate-800">
          {memberName}
        </p>

        <p className="mt-0.5 truncate font-mono text-[10px] text-slate-400">
          {member.userId}
        </p>
      </div>

      {/* Role */}
      <div className="flex shrink-0 items-center gap-1.5">
        {isOwner ? (
          <span
            className="
              inline-flex
              items-center
              gap-1
              rounded-full
              bg-slate-100
              px-2
              py-1
              text-[10px]
              font-medium
              text-slate-600
            "
          >
            <Check className="h-3 w-3" />
            Owner
          </span>
        ) : (
          <span
            className="
              rounded-full
              bg-slate-50
              px-2
              py-1
              text-[10px]
              font-medium
              text-slate-500
            "
          >
            {member.role || "Member"}
          </span>
        )}
      </div>
    </div>
  );
}

/* -------------------------------------------------------------------------- */
/* Quick Info                                                                 */
/* -------------------------------------------------------------------------- */

function QuickInfo({
  icon,
  label,
  value,
}: {
  icon: ReactNode;
  label: string;
  value: string;
}) {
  return (
    <div className="flex items-center gap-3 px-6 py-5">
      <div
        className="
          flex
          h-9
          w-9
          shrink-0
          items-center
          justify-center
          rounded-lg
          bg-slate-50
          text-slate-500
          ring-1
          ring-slate-200
        "
      >
        {icon}
      </div>

      <div className="min-w-0">
        <p className="text-[11px] font-semibold uppercase tracking-wider text-slate-400">
          {label}
        </p>

        <p className="mt-0.5 truncate text-sm font-medium text-slate-800">
          {value}
        </p>
      </div>
    </div>
  );
}

/* -------------------------------------------------------------------------- */
/* Status Badge                                                               */
/* -------------------------------------------------------------------------- */

function ProjectStatus({
  status,
}: {
  status: string | null;
}) {
  const normalized =
    status?.toLowerCase().trim() ?? "";

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

      {status || "Unknown"}
    </span>
  );
}

/* -------------------------------------------------------------------------- */
/* Visibility Badge                                                           */
/* -------------------------------------------------------------------------- */

function ProjectVisibility({
  visibility,
}: {
  visibility: string | null;
}) {
  return (
    <span
      className="
        inline-flex
        rounded-full
        bg-slate-100
        px-2.5
        py-1
        text-xs
        font-medium
        text-slate-600
      "
    >
      {visibility || "Unknown"}
    </span>
  );
}

/* -------------------------------------------------------------------------- */
/* Helpers                                                                    */
/* -------------------------------------------------------------------------- */

function getProjectInitials(
  project: ProjectDetail,
) {
  if (project.key?.trim()) {
    return project.key
      .slice(0, 2)
      .toUpperCase();
  }

  if (project.name?.trim()) {
    return project.name
      .slice(0, 2)
      .toUpperCase();
  }

  return "PR";
}

function getMemberInitials(
  name: string,
) {
  const value = name.trim();

  if (!value) {
    return "U";
  }

  const parts = value
    .split(/\s+/)
    .filter(Boolean);

  if (parts.length >= 2) {
    return (
      parts[0][0] +
      parts[parts.length - 1][0]
    ).toUpperCase();
  }

  return value
    .slice(0, 2)
    .toUpperCase();
}