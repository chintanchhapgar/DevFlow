import { Shield } from "lucide-react";

import { SessionsList } from "../components/SessionsList";

export function SecurityPage() {
  return (
    <div className="mx-auto w-full max-w-5xl space-y-6">

      {/* Page Header */}
      <div>
        <p className="text-sm font-medium text-blue-600">
          Account
        </p>

        <div className="mt-1 flex items-start gap-3">

          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-blue-50">
            <Shield className="h-5 w-5 text-blue-600" />
          </div>

          <div>
            <h1 className="text-2xl font-semibold tracking-tight text-slate-900">
              Security
            </h1>

            <p className="mt-1 text-sm text-slate-500">
              Manage your account security and active sessions.
            </p>
          </div>

        </div>
      </div>

      {/* Active Sessions */}
      <section className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">

        {/* Section Header */}
        <div className="border-b border-slate-100 px-6 py-5">

          <h2 className="text-sm font-semibold text-slate-900">
            Active Sessions
          </h2>

          <p className="mt-1 text-xs text-slate-500">
            Review devices currently signed in to your DevFlow account.
          </p>

        </div>

        {/* Sessions */}
        <div className="p-6">
          <SessionsList />
        </div>

      </section>

    </div>
  );
}