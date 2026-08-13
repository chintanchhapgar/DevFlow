import { useState } from "react";
import {
  LoaderCircle,
  Trash2,
  UserPlus,
  Check,
  Copy,
} from "lucide-react";
import { Mail, XCircle } from "lucide-react";

import { useProjectInvitations } from "../hooks/use-project";
import { useRevokeProjectInvitation } from "../hooks/use-project-mutations";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

import {
  ProjectRole,
  type ProjectDetail,
  type ProjectMember,
} from "../api/projects-api";
import {
  useInviteProjectMember,
  useRemoveProjectMember,
  useUpdateProjectMemberRole,
} from "../hooks/use-project-mutations";

type MemberDialogProps = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  project: ProjectDetail;
};

function roleFromApi(role: string): ProjectRole {
  switch (role.toLowerCase()) {
    case "administrator":
    case "admin":
      return ProjectRole.Administrator;
    case "guest":
      return ProjectRole.Guest;
    case "owner":
      return ProjectRole.Owner;
    default:
      return ProjectRole.Member;
  }
}

function roleLabel(role: ProjectRole): string {
  switch (role) {
    case ProjectRole.Owner:
      return "Owner";
    case ProjectRole.Administrator:
      return "Administrator";
    case ProjectRole.Guest:
      return "Guest";
    default:
      return "Member";
  }
}

export function MemberDialog({
  open,
  onOpenChange,
  project,
}: MemberDialogProps) {
  const inviteMember = useInviteProjectMember();
  const updateMemberRole = useUpdateProjectMemberRole();
  const removeMember = useRemoveProjectMember();

  const invitationsQuery = useProjectInvitations(
    project.projectId,
    );
    const revokeInvitation = useRevokeProjectInvitation();

  const [email, setEmail] = useState("");
  const [role, setRole] = useState<ProjectRole>(
    ProjectRole.Member,
  );
  const [error, setError] = useState<string | null>(null);
  const [memberToRemove, setMemberToRemove] =
    useState<ProjectMember | null>(null);
  const [inviteLink, setInviteLink] = useState<string | null>(null,);
    const [copied, setCopied] = useState(false);
  const members = project.members ?? [];
  const isBusy =
    inviteMember.isPending ||
    updateMemberRole.isPending ||
    removeMember.isPending ||
    revokeInvitation.isPending;

  async function handleInvite(
    event: React.FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    const value = email.trim();

    if (!value) {
      setError("An email address is required.");
      return;
    }

    setError(null);

    try {
      const invitation = await inviteMember.mutateAsync({
        projectId: project.projectId,
        request: {
            email: value,
            role,
        },
        });

        setInviteLink(
        `${window.location.origin}/invitations/respond?token=${encodeURIComponent(
            invitation.token,
        )}`,
        );
        setCopied(false);
        setEmail("");
        setRole(ProjectRole.Member);

      setEmail("");
      setRole(ProjectRole.Member);
    } catch {
      setError(
        "Unable to send the invitation. Check the email address and try again.",
      );
    }
  }

  async function handleCopyInviteLink() {
    if (!inviteLink) {
        return;
    }

    try {
        await navigator.clipboard.writeText(inviteLink);
        setCopied(true);
    } catch {
        setError(
        "Unable to copy the link. Please select and copy it manually.",
        );
    }
    }

  async function handleRoleChange(
    member: ProjectMember,
    nextRole: ProjectRole,
  ) {
    if (
      member.userId === project.ownerId ||
      roleFromApi(member.role) === nextRole
    ) {
      return;
    }

    setError(null);

    try {
      await updateMemberRole.mutateAsync({
        projectId: project.projectId,
        userId: member.userId,
        request: { role: nextRole },
      });
    } catch {
      setError("Unable to update the member role.");
    }
  }

  async function handleRemove() {
    if (!memberToRemove) {
      return;
    }

    setError(null);

    try {
      await removeMember.mutateAsync({
        projectId: project.projectId,
        userId: memberToRemove.userId,
      });

      setMemberToRemove(null);
    } catch {
      setError("Unable to remove this member.");
    }
  }

  async function handleRevokeInvitation(
    invitationId: string,
    ) {
    setError(null);

    try {
        await revokeInvitation.mutateAsync({
        projectId: project.projectId,
        invitationId,
        });
    } catch {
        setError("Unable to revoke this invitation.");
    }
    }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-h-[90vh] overflow-y-auto sm:max-w-2xl">
        <DialogHeader>
          <DialogTitle>Manage members</DialogTitle>

          <DialogDescription>
            Invite people and manage access to {project.name}.
          </DialogDescription>
        </DialogHeader>

        {memberToRemove ? (
          <div className="space-y-4">
            <div className="rounded-lg border border-red-200 bg-red-50 p-4">
              <p className="font-medium text-red-800">
                Remove this member?
              </p>

              <p className="mt-1 text-sm text-red-700">
                They will lose access to this project and its
                work items.
              </p>
            </div>

            {error && (
              <p className="text-sm text-red-600">{error}</p>
            )}

            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                disabled={isBusy}
                onClick={() => setMemberToRemove(null)}
              >
                Cancel
              </Button>

              <Button
                type="button"
                variant="destructive"
                disabled={isBusy}
                onClick={handleRemove}
              >
                {removeMember.isPending && (
                  <LoaderCircle className="h-4 w-4 animate-spin" />
                )}
                Remove member
              </Button>
            </DialogFooter>
          </div>
        ) : (
          <>
            <form
              className="rounded-xl border border-slate-200 bg-slate-50 p-4"
              onSubmit={handleInvite}
            >
              <div className="flex items-center gap-2">
                <UserPlus className="h-4 w-4 text-slate-500" />
                <h3 className="text-sm font-semibold text-slate-900">
                  Invite a member
                </h3>
              </div>

              <div className="mt-4 grid gap-3 sm:grid-cols-[1fr_160px_auto]">
                <div className="space-y-2">
                  <Label htmlFor="member-email">
                    Email address
                  </Label>

                  <Input
                    id="member-email"
                    type="email"
                    value={email}
                    placeholder="name@example.com"
                    disabled={isBusy}
                    onChange={(event) =>
                      setEmail(event.target.value)
                    }
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="invite-role">Role</Label>

                  <select
                    id="invite-role"
                    value={role}
                    disabled={isBusy}
                    onChange={(event) =>
                      setRole(
                        Number(
                          event.target.value,
                        ) as ProjectRole,
                      )
                    }
                    className="flex h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                  >
                    <option value={ProjectRole.Administrator}>
                      Administrator
                    </option>
                    <option value={ProjectRole.Member}>
                      Member
                    </option>
                    <option value={ProjectRole.Guest}>
                      Guest
                    </option>
                  </select>
                </div>

                <Button
                  type="submit"
                  disabled={isBusy}
                  className="self-end"
                >
                  {inviteMember.isPending && (
                    <LoaderCircle className="h-4 w-4 animate-spin" />
                  )}
                  Invite
                </Button>
              </div>
            </form>
            
            {inviteLink && (
                <div className="rounded-xl border border-blue-200 bg-blue-50 p-4">
                    <p className="text-sm font-semibold text-blue-900">
                    Invitation created
                    </p>

                    <p className="mt-1 text-sm text-blue-800">
                    Share this link with the invited member.
                    </p>

                    <div className="mt-3 flex gap-2">
                    <Input
                        readOnly
                        value={inviteLink}
                        onFocus={(event) => event.currentTarget.select()}
                        className="min-w-0 bg-white text-xs"
                        aria-label="Invitation link"
                    />

                    <Button
                        type="button"
                        variant="outline"
                        size="sm"
                        onClick={handleCopyInviteLink}
                        className="shrink-0 border-blue-200 bg-white text-blue-700 hover:bg-blue-100"
                    >
                        {copied ? (
                        <Check className="h-4 w-4" />
                        ) : (
                        <Copy className="h-4 w-4" />
                        )}
                        {copied ? "Copied" : "Copy"}
                    </Button>
                    </div>
                </div>
                )}
            {error && (
              <p className="text-sm text-red-600">{error}</p>
            )}

            <div className="overflow-hidden rounded-xl border border-slate-200">
              <div className="border-b border-slate-200 bg-slate-50 px-4 py-3">
                <h3 className="text-sm font-semibold text-slate-900">
                  Project members ({members.length})
                </h3>
              </div>

              {members.length === 0 ? (
                <div className="px-4 py-10 text-center text-sm text-slate-500">
                  No members have joined this project yet.
                </div>
              ) : (
                <div className="divide-y divide-slate-100">
                  {members.map((member) => {
                    const isOwner =
                      member.userId === project.ownerId;

                    return (
                      <div
                        key={member.userId}
                        className="flex flex-col gap-3 px-4 py-3 sm:flex-row sm:items-center"
                      >
                        <div className="min-w-0 flex-1">
                          <p className="truncate text-sm font-medium text-slate-800">
                            {member.memberName || "Unknown user"}
                          </p>

                          <p className="truncate font-mono text-xs text-slate-400">
                            {member.userId}
                          </p>
                        </div>

                        {isOwner ? (
                          <span className="text-sm font-medium text-slate-500">
                            Owner
                          </span>
                        ) : (
                          <>
                            <select
                              aria-label={`Role for ${
                                member.memberName || member.userId
                              }`}
                              value={roleFromApi(member.role)}
                              disabled={isBusy}
                              onChange={(event) =>
                                handleRoleChange(
                                  member,
                                  Number(
                                    event.target.value,
                                  ) as ProjectRole,
                                )
                              }
                              className="h-9 rounded-md border border-slate-200 bg-white px-2 text-sm outline-none focus:border-slate-400 focus:ring-2 focus:ring-slate-200"
                            >
                              {[
                                ProjectRole.Administrator,
                                ProjectRole.Member,
                                ProjectRole.Guest,
                              ].map((projectRole) => (
                                <option
                                  key={projectRole}
                                  value={projectRole}
                                >
                                  {roleLabel(projectRole)}
                                </option>
                              ))}
                            </select>

                            <Button
                              type="button"
                              variant="ghost"
                              size="icon"
                              disabled={isBusy}
                              aria-label={`Remove ${
                                member.memberName || "member"
                              }`}
                              onClick={() =>
                                setMemberToRemove(member)
                              }
                            >
                              <Trash2 className="h-4 w-4 text-red-600" />
                            </Button>
                          </>
                        )}
                      </div>
                    );
                  })}
                </div>
              )}
            </div>
            
            <section className="overflow-hidden rounded-xl border border-slate-200">
                <div className="border-b border-slate-200 bg-slate-50 px-4 py-3">
                    <h3 className="text-sm font-semibold text-slate-900">
                    Pending invitations
                    </h3>
                </div>

                {invitationsQuery.isLoading && (
                    <div className="space-y-3 p-4">
                    {[0, 1].map((index) => (
                        <div
                        key={index}
                        className="h-12 animate-pulse rounded-lg bg-slate-100"
                        />
                    ))}
                    </div>
                )}

                {invitationsQuery.isError && (
                    <div className="p-4">
                    <p className="text-sm text-red-600">
                        Unable to load invitations.
                    </p>

                    <Button
                        type="button"
                        variant="outline"
                        size="sm"
                        className="mt-3"
                        onClick={() => invitationsQuery.refetch()}
                    >
                        Try again
                    </Button>
                    </div>
                )}

                {!invitationsQuery.isLoading &&
                    !invitationsQuery.isError && (
                    <>
                        {(invitationsQuery.data ?? []).filter(
                        (invitation) =>
                            invitation.status.toLowerCase() === "pending",
                        ).length === 0 ? (
                        <p className="px-4 py-8 text-center text-sm text-slate-500">
                            No pending invitations.
                        </p>
                        ) : (
                        <div className="divide-y divide-slate-100">
                            {(invitationsQuery.data ?? [])
                            .filter(
                                (invitation) =>
                                invitation.status.toLowerCase() === "pending",
                            )
                            .map((invitation) => (
                                <div
                                key={invitation.invitationId}
                                className="flex items-center gap-3 px-4 py-3"
                                >
                                <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-blue-50 text-blue-600">
                                    <Mail className="h-4 w-4" />
                                </div>

                                <div className="min-w-0 flex-1">
                                    <p className="truncate text-sm font-medium text-slate-800">
                                    {invitation.email}
                                    </p>

                                    <p className="mt-0.5 text-xs text-slate-500">
                                    Invited as {invitation.role}
                                    </p>
                                </div>

                                <Button
                                    type="button"
                                    variant="ghost"
                                    size="sm"
                                    disabled={isBusy}
                                    onClick={() =>
                                    handleRevokeInvitation(
                                        invitation.invitationId,
                                    )
                                    }
                                    className="text-red-600 hover:bg-red-50 hover:text-red-700"
                                >
                                    <XCircle className="h-4 w-4" />
                                    Revoke
                                </Button>
                                </div>
                            ))}
                        </div>
                        )}
                    </>
                    )}
                </section>
            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                disabled={isBusy}
                onClick={() => onOpenChange(false)}
              >
                Close
              </Button>
            </DialogFooter>
          </>
        )}
      </DialogContent>
    </Dialog>
  );
}