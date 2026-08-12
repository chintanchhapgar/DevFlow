import { LogOut, ShieldCheck } from "lucide-react";

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
      <div className="flex flex-col items-center justify-center py-12">
        <div className="h-6 w-6 animate-spin rounded-full border-2 border-slate-200 border-t-blue-600" />

        <p className="mt-3 text-sm text-slate-500">
          Loading active sessions...
        </p>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-center">
        <p className="text-sm font-medium text-red-700">
          Unable to load active sessions.
        </p>

        <p className="mt-1 text-xs text-red-600/80">
          Please refresh the page and try again.
        </p>
      </div>
    );
  }

  const hasOtherSessions = sessions.some(
    (session) => !session.isCurrent,
  );

  return (
    <div className="space-y-5">

      {/* Security Information */}
      <div className="flex items-start gap-3 rounded-xl border border-blue-100 bg-blue-50 p-4">

        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-white">
          <ShieldCheck className="h-4 w-4 text-blue-600" />
        </div>

        <div>
          <p className="text-sm font-medium text-blue-900">
            Your account sessions
          </p>

          <p className="mt-1 text-xs leading-5 text-blue-700/80">
            These are the devices currently signed in to
            your DevFlow account. You can revoke access
            from any device you no longer recognize.
          </p>
        </div>

      </div>

      {/* Sessions */}
      {sessions.length === 0 ? (
        <div className="rounded-xl border border-slate-200 bg-slate-50 p-8 text-center">

          <p className="text-sm font-medium text-slate-700">
            No active sessions found.
          </p>

          <p className="mt-1 text-xs text-slate-500">
            Your active sessions will appear here.
          </p>

        </div>
      ) : (
        <div className="space-y-3">

          {sessions.map((session) => (
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
          ))}

        </div>
      )}

      {/* Revoke Other Sessions */}
      {hasOtherSessions && (
        <div className="flex flex-col gap-3 border-t border-slate-100 pt-5 sm:flex-row sm:items-center sm:justify-between">

          <div>
            <p className="text-sm font-medium text-slate-800">
              Sign out other devices
            </p>

            <p className="mt-1 text-xs text-slate-500">
              Revoke access from every session except this
              device.
            </p>
          </div>

          <button
            type="button"
            disabled={revokeOthersMutation.isPending}
            onClick={() =>
              revokeOthersMutation.mutate()
            }
            className="
              inline-flex
              shrink-0
              items-center
              justify-center
              gap-2
              rounded-lg
              border
              border-red-200
              bg-white
              px-4
              py-2
              text-sm
              font-medium
              text-red-600
              shadow-sm
              transition
              hover:bg-red-50
              hover:text-red-700
              focus:outline-none
              focus:ring-2
              focus:ring-red-500/20
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