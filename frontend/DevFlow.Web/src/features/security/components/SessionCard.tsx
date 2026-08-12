import {
  Monitor,
  Smartphone,
  LogOut,
  Globe,
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
  const deviceName =
    session.deviceName?.toLowerCase() ?? "";

  const DeviceIcon =
    deviceName.includes("mobile") ||
    deviceName.includes("phone") ||
    deviceName.includes("android") ||
    deviceName.includes("iphone")
      ? Smartphone
      : Monitor;

  const lastActive = session.lastUsedAtUtc
    ? new Date(
        session.lastUsedAtUtc,
      ).toLocaleString()
    : null;

  return (
    <div
      className={[
        "rounded-xl border bg-white p-5 shadow-sm transition",
        session.isCurrent
          ? "border-blue-200 ring-1 ring-blue-50"
          : "border-slate-200 hover:border-slate-300",
      ].join(" ")}
    >
      <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">

        {/* Session Information */}
        <div className="flex min-w-0 gap-4">

          {/* Device Icon */}
          <div
            className={[
              "flex h-11 w-11 shrink-0 items-center justify-center rounded-lg",
              session.isCurrent
                ? "bg-blue-50"
                : "bg-slate-100",
            ].join(" ")}
          >
            <DeviceIcon
              className={[
                "h-5 w-5",
                session.isCurrent
                  ? "text-blue-600"
                  : "text-slate-600",
              ].join(" ")}
            />
          </div>

          {/* Details */}
          <div className="min-w-0">

            {/* Browser + Current */}
            <div className="flex flex-wrap items-center gap-2">

              <h3 className="text-sm font-semibold text-slate-900">
                {session.browser ?? "Unknown browser"}
              </h3>

              {session.isCurrent && (
                <span className="inline-flex items-center rounded-full bg-emerald-50 px-2.5 py-1 text-xs font-medium text-emerald-700">
                  Current device
                </span>
              )}

            </div>

            {/* Operating System */}
            <p className="mt-1 text-sm text-slate-500">
              {session.operatingSystem ??
                "Unknown operating system"}
            </p>

            {/* Metadata */}
            <div className="mt-3 space-y-1.5 text-xs text-slate-400">

              {session.ipAddress && (
                <div className="flex items-center gap-2">
                  <Globe className="h-3.5 w-3.5" />

                  <span>
                    {session.ipAddress}
                  </span>
                </div>
              )}

              {lastActive && (
                <p>
                  Last active{" "}
                  <span className="text-slate-500">
                    {lastActive}
                  </span>
                </p>
              )}

            </div>

          </div>
        </div>

        {/* Revoke */}
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
              justify-center
              gap-2
              rounded-lg
              border
              border-red-200
              bg-white
              px-3
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

            {isRevoking
              ? "Revoking..."
              : "Revoke"}
          </button>
        )}

      </div>
    </div>
  );
}