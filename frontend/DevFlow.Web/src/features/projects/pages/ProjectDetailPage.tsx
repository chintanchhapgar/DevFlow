import {
  useEffect,
  useMemo,
  useState,
} from "react";
import {
  AlertCircle,
  FileText,
  FolderKanban,
  LoaderCircle,
  Pencil,
  Plus,
  Users,
} from "lucide-react";
import { useParams } from "react-router-dom";

import { Button } from "@/components/ui/button";

import { downloadAttachment } from "../api/project-resources-api";
import { AttachmentsPanel } from "../components/AttachmentsPanel";
import { MemberDialog } from "../components/MemberDialog";
import { ProjectDialog } from "../components/ProjectDialog";
import { WorkItemDialog } from "../components/WorkItemDialog";
import { useProject } from "../hooks/use-project";
import { useProjectWorkItems } from "../hooks/use-project-resources";

type Tab = "overview" | "members" | "work" | "attachments";

export function ProjectDetailPage() {
  const { projectId } = useParams<{ projectId: string }>();

  const projectQuery = useProject(projectId);
  const workItemsQuery = useProjectWorkItems(projectId);

  const [activeTab, setActiveTab] =
    useState<Tab>("overview");
  const [isEditProjectOpen, setIsEditProjectOpen] =
    useState(false);
  const [isMembersOpen, setIsMembersOpen] =
    useState(false);
  const [isCreateWorkItemOpen, setIsCreateWorkItemOpen] =
    useState(false);
  const [workItemToEditId, setWorkItemToEditId] =
    useState<string | null>(null);
  const [selectedWorkItemId, setSelectedWorkItemId] =
    useState<string | null>(null);

  const project = projectQuery.data;
  const workItems = workItemsQuery.data?.items ?? [];

  const selectedWorkItem = useMemo(
    () =>
      workItems.find(
        (workItem) => workItem.id === selectedWorkItemId,
      ) ?? null,
    [selectedWorkItemId, workItems],
  );

  useEffect(() => {
    if (
      selectedWorkItemId &&
      !workItems.some(
        (workItem) => workItem.id === selectedWorkItemId,
      )
    ) {
      setSelectedWorkItemId(null);
    }
  }, [selectedWorkItemId, workItems]);

  if (projectQuery.isLoading) {
    return <ProjectDetailSkeleton />;
  }

  if (projectQuery.isError || !project) {
    return (
      <div className="mx-auto flex w-full max-w-7xl flex-col items-center justify-center rounded-2xl border border-red-200 bg-red-50 px-5 py-16 text-center">
        <AlertCircle className="h-8 w-8 text-red-600" />

        <h1 className="mt-3 text-lg font-semibold text-red-900">
          Unable to load project
        </h1>

        <p className="mt-1 text-sm text-red-700">
          The project may not exist, or you may not have access.
        </p>

        <Button
          type="button"
          variant="outline"
          className="mt-5"
          onClick={() => projectQuery.refetch()}
        >
          Try again
        </Button>
      </div>
    );
  }

  const members = project.members ?? [];
  const workItemToEdit =
    workItems.find(
      (workItem) => workItem.id === workItemToEditId,
    ) ?? null;

  return (
    <div className="mx-auto w-full max-w-7xl space-y-6">
      <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm sm:p-6">
        <div className="flex flex-col gap-5 lg:flex-row lg:items-start lg:justify-between">
          <div className="flex min-w-0 items-start gap-4">
            <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-xl bg-slate-100 text-sm font-bold text-slate-600">
              {project.key.slice(0, 2).toUpperCase()}
            </div>

            <div className="min-w-0">
              <div className="flex flex-wrap items-center gap-2">
                <h1 className="truncate text-2xl font-semibold tracking-tight text-slate-900">
                  {project.name}
                </h1>

                <Badge value={project.status} />
                <Badge value={project.visibility} />
              </div>

              <p className="mt-2 max-w-3xl text-sm text-slate-500">
                {project.description ||
                  "No project description has been added."}
              </p>

              <p className="mt-3 text-xs text-slate-400">
                Key: {project.key}
              </p>
            </div>
          </div>

          <Button
            type="button"
            variant="outline"
            onClick={() => setIsEditProjectOpen(true)}
          >
            <Pencil className="h-4 w-4" />
            Edit project
          </Button>
        </div>

        <div className="mt-6 flex gap-5 overflow-x-auto border-t border-slate-100">
          <TabButton
            active={activeTab === "overview"}
            onClick={() => setActiveTab("overview")}
          >
            Overview
          </TabButton>

          <TabButton
            active={activeTab === "members"}
            onClick={() => setActiveTab("members")}
          >
            Members ({members.length})
          </TabButton>

          <TabButton
            active={activeTab === "work"}
            onClick={() => setActiveTab("work")}
          >
            Work ({workItemsQuery.data?.totalCount ?? 0})
          </TabButton>

          <TabButton
            active={activeTab === "attachments"}
            onClick={() => setActiveTab("attachments")}
          >
            Attachments
          </TabButton>
        </div>
      </section>

      {activeTab === "overview" && (
        <OverviewTab
          project={project}
          memberCount={members.length}
          workItemCount={workItemsQuery.data?.totalCount ?? 0}
          onManageMembers={() => setIsMembersOpen(true)}
          onViewWork={() => setActiveTab("work")}
        />
      )}

      {activeTab === "members" && (
        <MembersTab
          members={members}
          ownerId={project.ownerId}
          onManage={() => setIsMembersOpen(true)}
        />
      )}

      {activeTab === "work" && (
        <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
          <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between">
            <div>
              <h2 className="text-base font-semibold text-slate-900">
                Work items
              </h2>

              <p className="mt-1 text-sm text-slate-500">
                Plan and track the work in this project.
              </p>
            </div>

            <Button
              type="button"
              size="sm"
              onClick={() => setIsCreateWorkItemOpen(true)}
            >
              <Plus className="h-4 w-4" />
              New work item
            </Button>
          </div>

          {workItemsQuery.isLoading && (
            <div className="space-y-3 p-5">
              {[0, 1, 2, 3].map((index) => (
                <div
                  key={index}
                  className="h-16 animate-pulse rounded-lg bg-slate-100"
                />
              ))}
            </div>
          )}

          {workItemsQuery.isError && (
            <div className="p-5">
              <div className="rounded-lg border border-red-200 bg-red-50 p-4">
                <p className="font-medium text-red-800">
                  Unable to load work items.
                </p>

                <Button
                  type="button"
                  variant="outline"
                  size="sm"
                  className="mt-3"
                  onClick={() => workItemsQuery.refetch()}
                >
                  Try again
                </Button>
              </div>
            </div>
          )}

          {!workItemsQuery.isLoading &&
            !workItemsQuery.isError &&
            workItems.length === 0 && (
              <EmptyState
                icon={<FileText className="h-6 w-6" />}
                title="No work items yet"
                description="Create a work item to start tracking work."
                action="New work item"
                onAction={() => setIsCreateWorkItemOpen(true)}
              />
            )}

          {!workItemsQuery.isLoading &&
            !workItemsQuery.isError &&
            workItems.length > 0 && (
              <div className="divide-y divide-slate-100">
                {workItems.map((workItem) => (
                  <button
                    key={workItem.id}
                    type="button"
                    className="flex w-full items-center gap-3 px-5 py-4 text-left transition-colors hover:bg-slate-50"
                    onClick={() => setWorkItemToEditId(workItem.id)}
                  >
                    <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-slate-100 text-slate-500">
                      <FileText className="h-4 w-4" />
                    </div>

                    <div className="min-w-0 flex-1">
                      <p className="truncate text-sm font-medium text-slate-900">
                        {workItem.title}
                      </p>

                      <p className="mt-0.5 text-xs text-slate-500">
                        {workItem.key} · {enumLabel(workItem.status)}
                        {" · "}
                        {enumLabel(workItem.priority)}
                      </p>
                    </div>

                    <Button
                      type="button"
                      variant="outline"
                      size="sm"
                      onClick={(event) => {
                        event.stopPropagation();
                        setSelectedWorkItemId(workItem.id);
                        setActiveTab("attachments");
                      }}
                    >
                      Attachments
                    </Button>
                  </button>
                ))}
              </div>
            )}
        </section>
      )}

      {activeTab === "attachments" && (
        <div className="grid gap-6 lg:grid-cols-[280px_minmax(0,1fr)]">
          <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
            <div className="border-b border-slate-100 px-4 py-4">
              <h2 className="font-semibold text-slate-900">
                Work items
              </h2>
            </div>

            {workItems.length === 0 ? (
              <p className="px-4 py-8 text-sm text-slate-500">
                No work items are available.
              </p>
            ) : (
              <div className="divide-y divide-slate-100">
                {workItems.map((workItem) => (
                  <button
                    key={workItem.id}
                    type="button"
                    onClick={() =>
                      setSelectedWorkItemId(workItem.id)
                    }
                    className={`w-full px-4 py-3 text-left text-sm transition-colors ${
                      selectedWorkItemId === workItem.id
                        ? "bg-slate-100 text-slate-900"
                        : "text-slate-600 hover:bg-slate-50"
                    }`}
                  >
                    <span className="block truncate font-medium">
                      {workItem.title}
                    </span>

                    <span className="mt-0.5 block text-xs text-slate-400">
                      {workItem.key}
                    </span>
                  </button>
                ))}
              </div>
            )}
          </section>

          <AttachmentsPanel
            workItemId={selectedWorkItem?.id}
            workItemTitle={selectedWorkItem?.title}
          />
        </div>
      )}

      <ProjectDialog
        mode="edit"
        open={isEditProjectOpen}
        onOpenChange={setIsEditProjectOpen}
        project={project}
      />

      <MemberDialog
        open={isMembersOpen}
        onOpenChange={setIsMembersOpen}
        project={project}
      />

      <WorkItemDialog
        mode="create"
        open={isCreateWorkItemOpen}
        onOpenChange={setIsCreateWorkItemOpen}
        projectId={project.projectId}
      />

      {workItemToEdit && (
        <WorkItemDialog
          mode="edit"
          open
          onOpenChange={(open) => {
            if (!open) {
              setWorkItemToEditId(null);
            }
          }}
          projectId={project.projectId}
          workItem={workItemToEdit}
        />
      )}
    </div>
  );
}

function OverviewTab({
  project,
  memberCount,
  workItemCount,
  onManageMembers,
  onViewWork,
}: {
  project: {
    key: string;
    ownerName: string;
    ownerId: string;
    visibility: string;
    status: string;
  };
  memberCount: number;
  workItemCount: number;
  onManageMembers: () => void;
  onViewWork: () => void;
}) {
  return (
    <div className="grid gap-6 lg:grid-cols-3">
      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm lg:col-span-2">
        <div className="border-b border-slate-100 px-5 py-4">
          <h2 className="font-semibold text-slate-900">
            Project overview
          </h2>
        </div>

        <dl className="grid gap-px bg-slate-100 sm:grid-cols-2">
          <Info label="Project key" value={project.key} />
          <Info label="Status" value={project.status} />
          <Info label="Visibility" value={project.visibility} />
          <Info label="Owner" value={project.ownerName} />
          <Info label="Owner ID" value={project.ownerId} mono />
        </dl>
      </section>

      <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
        <h2 className="font-semibold text-slate-900">
          Quick actions
        </h2>

        <div className="mt-4 space-y-3">
          <Button
            type="button"
            variant="outline"
            className="w-full justify-start"
            onClick={onManageMembers}
          >
            <Users className="h-4 w-4" />
            Manage {memberCount} members
          </Button>

          <Button
            type="button"
            variant="outline"
            className="w-full justify-start"
            onClick={onViewWork}
          >
            <FolderKanban className="h-4 w-4" />
            View {workItemCount} work items
          </Button>
        </div>
      </section>
    </div>
  );
}

function MembersTab({
  members,
  ownerId,
  onManage,
}: {
  members: {
    userId: string;
    memberName: string;
    role: string;
  }[];
  ownerId: string;
  onManage: () => void;
}) {
  return (
    <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
      <div className="flex items-center justify-between border-b border-slate-100 px-5 py-4">
        <div>
          <h2 className="font-semibold text-slate-900">
            Members
          </h2>

          <p className="mt-1 text-sm text-slate-500">
            People with access to this project.
          </p>
        </div>

        <Button type="button" size="sm" onClick={onManage}>
          Manage members
        </Button>
      </div>

      {members.length === 0 ? (
        <EmptyState
          icon={<Users className="h-6 w-6" />}
          title="No members yet"
          description="Invite people to collaborate on this project."
          action="Manage members"
          onAction={onManage}
        />
      ) : (
        <div className="divide-y divide-slate-100">
          {members.map((member) => (
            <div
              key={member.userId}
              className="flex items-center gap-3 px-5 py-4"
            >
              <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-slate-100 text-sm font-semibold text-slate-600">
                {initials(member.memberName)}
              </div>

              <div className="min-w-0 flex-1">
                <p className="truncate text-sm font-medium text-slate-900">
                  {member.memberName || "Unknown user"}
                </p>

                <p className="truncate text-xs text-slate-500">
                  {member.userId}
                </p>
              </div>

              <Badge
                value={
                  member.userId === ownerId
                    ? "Owner"
                    : member.role
                }
              />
            </div>
          ))}
        </div>
      )}
    </section>
  );
}

function TabButton({
  active,
  children,
  onClick,
}: {
  active: boolean;
  children: React.ReactNode;
  onClick: () => void;
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={`relative whitespace-nowrap py-3 text-sm font-medium ${
        active
          ? "text-slate-900 after:absolute after:inset-x-0 after:bottom-0 after:h-0.5 after:bg-slate-900"
          : "text-slate-400 hover:text-slate-700"
      }`}
    >
      {children}
    </button>
  );
}

function Badge({ value }: { value: string }) {
  return (
    <span className="rounded-full bg-slate-100 px-2.5 py-1 text-xs font-medium text-slate-600">
      {value || "Unknown"}
    </span>
  );
}

function Info({
  label,
  value,
  mono = false,
}: {
  label: string;
  value: string;
  mono?: boolean;
}) {
  return (
    <div className="bg-white px-5 py-4">
      <dt className="text-[11px] font-semibold uppercase tracking-wider text-slate-400">
        {label}
      </dt>

      <dd
        className={`mt-1 truncate text-sm text-slate-700 ${
          mono ? "font-mono text-xs" : "font-medium"
        }`}
      >
        {value}
      </dd>
    </div>
  );
}

function EmptyState({
  icon,
  title,
  description,
  action,
  onAction,
}: {
  icon: React.ReactNode;
  title: string;
  description: string;
  action: string;
  onAction: () => void;
}) {
  return (
    <div className="flex min-h-[250px] flex-col items-center justify-center px-5 text-center">
      <div className="text-slate-400">{icon}</div>

      <h3 className="mt-3 text-sm font-semibold text-slate-900">
        {title}
      </h3>

      <p className="mt-1 max-w-sm text-sm text-slate-500">
        {description}
      </p>

      <Button
        type="button"
        variant="outline"
        size="sm"
        className="mt-4"
        onClick={onAction}
      >
        {action}
      </Button>
    </div>
  );
}

function ProjectDetailSkeleton() {
  return (
    <div className="mx-auto w-full max-w-7xl space-y-6">
      <div className="h-52 animate-pulse rounded-2xl bg-slate-100" />

      <div className="grid gap-6 lg:grid-cols-3">
        <div className="h-64 animate-pulse rounded-2xl bg-slate-100 lg:col-span-2" />
        <div className="h-64 animate-pulse rounded-2xl bg-slate-100" />
      </div>
    </div>
  );
}

function initials(value: string) {
  return value
    .trim()
    .split(/\s+/)
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part[0])
    .join("")
    .toUpperCase() || "U";
}

function enumLabel(value: string | number) {
  if (typeof value === "string") {
    return value;
  }

  return String(value);
}