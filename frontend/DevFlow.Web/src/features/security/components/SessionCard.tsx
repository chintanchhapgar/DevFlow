import {
  Globe,
  LogOut,
  Monitor,
  Smartphone,
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

  return (
    <div
      className="
        rounded-xl
        border
        border-slate-200
        bg-white
        p-5
        shadow-sm
        transition-shadow
        hover:shadow-md
      "
    >
      <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
        {/* Session Information */}
        <div className="flex min-w-0 gap-4">
          {/* Device Icon */}
          <div
            className="
              flex
              h-11
              w-11
              shrink-0
              items-center
              justify-center
              rounded-xl
              bg-[#eef3f8]
            "
          >
            <DeviceIcon className="h-5 w-5 text-[#456b9a]" />
          </div>

          {/* Details */}
          <div className="min-w-0">
            {/* Browser + Current */}
            <div className="flex flex-wrap items-center gap-2">
              <h3 className="truncate text-sm font-semibold text-slate-900">
                {session.browser ??
                  "Unknown browser"}
              </h3>

              {session.isCurrent && (
                <span
                  className="
                    inline-flex
                    items-center
                    gap-1.5
                    rounded-full
                    bg-emerald-50
                    px-2.5
                    py-1
                    text-[11px]
                    font-semibold
                    text-emerald-700
                  "
                >
                  <span className="h-1.5 w-1.5 rounded-full bg-emerald-500" />

                  Current
                </span>
              )}
            </div>

            {/* Operating System */}
            <p className="mt-2 text-sm text-slate-600">
              {session.operatingSystem ??
                "Unknown operating system"}
            </p>

            {/* Metadata */}
            <div className="mt-2 space-y-1">
              {session.ipAddress && (
                <div className="flex items-center gap-2 text-xs text-slate-400">
                  <Globe className="h-3.5 w-3.5" />

                  <span>
                    {session.ipAddress}
                  </span>
                </div>
              )}

              {session.lastUsedAtUtc && (
                <p className="text-xs text-slate-400">
                  Last active{" "}
                  {new Date(
                    session.lastUsedAtUtc,
                  ).toLocaleString()}
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
              w-full
              shrink-0
              items-center
              justify-center
              gap-2
              rounded-lg
              border
              border-slate-200
              bg-white
              px-3.5
              py-2
              text-sm
              font-medium
              text-slate-600
              shadow-sm
              transition-colors
              hover:border-red-200
              hover:bg-red-50
              hover:text-red-600
              focus:outline-none
              focus:ring-2
              focus:ring-red-500/10
              disabled:pointer-events-none
              disabled:opacity-50
              sm:w-auto
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