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
      <div className="flex min-h-32 items-center justify-center">
        <div className="flex items-center gap-3 text-sm text-slate-500">
          <div className="h-4 w-4 animate-spin rounded-full border-2 border-slate-200 border-t-[#456b9a]" />
          Loading active sessions...
        </div>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 px-5 py-4 text-center text-sm text-red-700">
        Unable to load active sessions.
      </div>
    );
  }

  if (sessions.length === 0) {
    return (
      <div className="rounded-xl border border-dashed border-slate-200 bg-slate-50 px-6 py-10 text-center">
        <div className="mx-auto flex h-10 w-10 items-center justify-center rounded-full bg-white shadow-sm ring-1 ring-slate-200">
          <LogOut className="h-4 w-4 text-slate-400" />
        </div>

        <p className="mt-3 text-sm font-medium text-slate-700">
          No active sessions found
        </p>

        <p className="mt-1 text-xs text-slate-500">
          Your active login sessions will appear here.
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-5">
      {/* Session Cards */}
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

      {/* Revoke All Other Sessions */}
      <div className="flex flex-col gap-4 border-t border-slate-200 pt-5 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <p className="text-sm font-semibold text-slate-800">
            Sign out other devices
          </p>

          <p className="mt-1 max-w-xl text-xs leading-5 text-slate-500">
            Revoke all active sessions except the device
            you are currently using.
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
            h-9
            shrink-0
            items-center
            justify-center
            gap-2
            rounded-lg
            border
            border-red-200
            bg-white
            px-4
            text-sm
            font-medium
            text-red-600
            shadow-sm
            transition-colors
            hover:bg-red-50
            hover:text-red-700
            focus:outline-none
            focus:ring-2
            focus:ring-red-200
            disabled:pointer-events-none
            disabled:opacity-50
          "
        >
          <LogOut className="h-4 w-4" />

          {revokeOthersMutation.isPending
            ? "Revoking..."
            : "Revoke All Other Sessions"}
        </button>
      </div>
    </div>
  );
}