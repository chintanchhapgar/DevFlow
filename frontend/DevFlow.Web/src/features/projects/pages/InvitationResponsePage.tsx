import { useState } from "react";
import {
  CheckCircle2,
  LoaderCircle,
  Mail,
  XCircle,
} from "lucide-react";
import {
  Link,
  useSearchParams,
} from "react-router-dom";

import { Button } from "@/components/ui/button";

import {
  useAcceptProjectInvitation,
  useDeclineProjectInvitation,
} from "../hooks/use-project-mutations";

export function InvitationResponsePage() {
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token");

  const acceptInvitation = useAcceptProjectInvitation();
  const declineInvitation = useDeclineProjectInvitation();

  const [result, setResult] = useState<
    "accepted" | "declined" | null
  >(null);
  const [error, setError] = useState<string | null>(null);

  const isPending =
    acceptInvitation.isPending ||
    declineInvitation.isPending;

  async function handleAccept() {
    if (!token) {
      return;
    }

    setError(null);

    try {
      await acceptInvitation.mutateAsync(token);
      setResult("accepted");
    } catch {
      setError(
        "Unable to accept this invitation. It may have expired or already been used.",
      );
    }
  }

  async function handleDecline() {
    if (!token) {
      return;
    }

    setError(null);

    try {
      await declineInvitation.mutateAsync(token);
      setResult("declined");
    } catch {
      setError(
        "Unable to decline this invitation. It may have expired or already been used.",
      );
    }
  }

  if (!token) {
    return (
      <InvitationCard
        icon={<XCircle className="h-8 w-8 text-red-600" />}
        title="Invalid invitation link"
        description="This invitation link is missing its token."
      />
    );
  }

  if (result === "accepted") {
    return (
      <InvitationCard
        icon={<CheckCircle2 className="h-8 w-8 text-emerald-600" />}
        title="Invitation accepted"
        description="You now have access to the project."
        action={
          <Button asChild>
            <Link to="/projects">View projects</Link>
          </Button>
        }
      />
    );
  }

  if (result === "declined") {
    return (
      <InvitationCard
        icon={<XCircle className="h-8 w-8 text-slate-500" />}
        title="Invitation declined"
        description="You will not be added to this project."
        action={
          <Button asChild variant="outline">
            <Link to="/projects">Back to projects</Link>
          </Button>
        }
      />
    );
  }

  return (
    <main className="flex min-h-[70vh] items-center justify-center px-4">
      <section className="w-full max-w-md rounded-2xl border border-slate-200 bg-white p-7 text-center shadow-sm">
        <div className="mx-auto flex h-12 w-12 items-center justify-center rounded-xl bg-blue-50 text-blue-600">
          <Mail className="h-6 w-6" />
        </div>

        <h1 className="mt-5 text-xl font-semibold tracking-tight text-slate-900">
          Project invitation
        </h1>

        <p className="mt-2 text-sm leading-6 text-slate-500">
          You have been invited to collaborate on a DevFlow
          project. Would you like to join?
        </p>

        {error && (
          <p className="mt-4 rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
            {error}
          </p>
        )}

        <div className="mt-6 flex flex-col-reverse gap-3 sm:flex-row sm:justify-center">
          <Button
            type="button"
            variant="outline"
            disabled={isPending}
            onClick={handleDecline}
          >
            {declineInvitation.isPending && (
              <LoaderCircle className="h-4 w-4 animate-spin" />
            )}
            Decline
          </Button>

          <Button
            type="button"
            disabled={isPending}
            onClick={handleAccept}
          >
            {acceptInvitation.isPending && (
              <LoaderCircle className="h-4 w-4 animate-spin" />
            )}
            Accept invitation
          </Button>
        </div>
      </section>
    </main>
  );
}

function InvitationCard({
  icon,
  title,
  description,
  action,
}: {
  icon: React.ReactNode;
  title: string;
  description: string;
  action?: React.ReactNode;
}) {
  return (
    <main className="flex min-h-[70vh] items-center justify-center px-4">
      <section className="w-full max-w-md rounded-2xl border border-slate-200 bg-white p-7 text-center shadow-sm">
        <div className="mx-auto flex h-12 w-12 items-center justify-center rounded-xl bg-slate-50">
          {icon}
        </div>

        <h1 className="mt-5 text-xl font-semibold tracking-tight text-slate-900">
          {title}
        </h1>

        <p className="mt-2 text-sm leading-6 text-slate-500">
          {description}
        </p>

        {action && <div className="mt-6">{action}</div>}
      </section>
    </main>
  );
}