import {
  Monitor,
  Smartphone,
  Globe,
  LogOut,
} from "lucide-react";

import type { Session } from "../types/session";

interface SessionCardProps {
  session: Session;
  onRevoke: (sessionId: string) => void;
  isRevoking: boolean;
}

export function SessionCard({
  session,
  onRevoke,
  isRevoking,
}: SessionCardProps) {
  const DeviceIcon =
    session.deviceName?.toLowerCase().includes("mobile") ||
    session.deviceName?.toLowerCase().includes("phone")
      ? Smartphone
      : Monitor;

  return (
    <div className="rounded-xl border border-white/10 bg-white/5 p-5">
      <div className="flex items-start justify-between gap-4">
        <div className="flex min-w-0 gap-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-white/5">
            <DeviceIcon className="h-5 w-5 text-slate-400" />
          </div>

          <div className="min-w-0">
            <div className="flex flex-wrap items-center gap-2">
              <h3 className="font-medium text-white">
                {session.browser ?? "Unknown browser"}
              </h3>

              {session.isCurrent && (
                <span className="rounded-full bg-emerald-500/10 px-2 py-1 text-xs font-medium text-emerald-400">
                  Current
                </span>
              )}
            </div>

            <div className="mt-2 space-y-1 text-sm text-slate-400">
              <p>
                {session.operatingSystem ??
                  "Unknown operating system"}
              </p>

              {session.ipAddress && (
                <p>{session.ipAddress}</p>
              )}

              {session.lastUsedAtUtc && (
                <p>
                  Last active{" "}
                  {new Date(
                    session.lastUsedAtUtc
                  ).toLocaleString()}
                </p>
              )}
            </div>
          </div>
        </div>

        {!session.isCurrent && (
          <button
            type="button"
            disabled={isRevoking}
            onClick={() =>
              onRevoke(session.sessionId)
            }
            className="
              inline-flex
              shrink-0
              items-center
              gap-2
              rounded-lg
              border
              border-red-500/20
              px-3
              py-2
              text-sm
              text-red-400
              transition
              hover:bg-red-500/10
              disabled:cursor-not-allowed
              disabled:opacity-50
            "
          >
            <LogOut className="h-4 w-4" />

            {isRevoking
              ? "Revoking..."
              : "Revoke"}
          </button>
        )}
      </div>
    </div>
  );
}