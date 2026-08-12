import { LogOut } from "lucide-react";

import {
  useRevokeOtherSessions,
  useRevokeSession,
  useSessions,
} from "../hooks/use-sessions";

import { SessionCard } from "./SessionCard";

export function SessionsList() {
  const {
    data: sessions = [],
    isLoading,
    isError,
  } = useSessions();

  const revokeMutation = useRevokeSession();
  const revokeOthersMutation =
    useRevokeOtherSessions();

  if (isLoading) {
    return (
      <div className="py-10 text-center text-sm text-slate-400">
        Loading active sessions...
      </div>
    );
  }

  if (isError) {
    return (
      <div className="py-10 text-center text-sm text-red-400">
        Unable to load active sessions.
      </div>
    );
  }

  return (
    <div className="space-y-4">
      {sessions.length === 0 ? (
        <div className="rounded-xl border border-white/10 bg-white/5 p-6 text-center text-sm text-slate-400">
          No active sessions found.
        </div>
      ) : (
        sessions.map((session) => (
          <SessionCard
            key={session.sessionId}
            session={session}
            onRevoke={(sessionId) =>
              revokeMutation.mutate(sessionId)
            }
            isRevoking={
              revokeMutation.isPending &&
              revokeMutation.variables ===
                session.sessionId
            }
          />
        ))
      )}

      {sessions.some(
        (session) => !session.isCurrent
      ) && (
        <div className="flex justify-end pt-2">
          <button
            type="button"
            disabled={revokeOthersMutation.isPending}
            onClick={() =>
              revokeOthersMutation.mutate()
            }
            className="
              inline-flex
              items-center
              gap-2
              rounded-lg
              border
              border-red-500/20
              px-4
              py-2
              text-sm
              font-medium
              text-red-400
              transition
              hover:bg-red-500/10
              disabled:cursor-not-allowed
              disabled:opacity-50
            "
          >
            <LogOut className="h-4 w-4" />

            {revokeOthersMutation.isPending
              ? "Revoking..."
              : "Revoke All Other Sessions"}
          </button>
        </div>
      )}
    </div>
  );
}