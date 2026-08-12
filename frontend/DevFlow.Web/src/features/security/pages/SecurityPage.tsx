import { Shield, LockKeyhole } from "lucide-react";

import { SessionsList } from "../components/SessionsList";

export function SecurityPage() {
  return (
    <div className="mx-auto w-full max-w-5xl space-y-8">
      {/* Page Header */}
      <div>
        <div className="flex items-start gap-4">
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
            <Shield className="h-5 w-5 text-[#456b9a]" />
          </div>

          <div>
            <h1 className="text-2xl font-bold tracking-tight text-slate-900 sm:text-3xl">
              Security
            </h1>

            <p className="mt-2 text-sm text-slate-500">
              Manage your account security and active sessions.
            </p>
          </div>
        </div>
      </div>

      {/* Security Overview */}
      <section className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
        <div className="flex items-start gap-4">
          <div
            className="
              flex
              h-10
              w-10
              shrink-0
              items-center
              justify-center
              rounded-xl
              bg-emerald-50
            "
          >
            <LockKeyhole className="h-5 w-5 text-emerald-600" />
          </div>

          <div>
            <h2 className="text-sm font-semibold text-slate-900">
              Your account is protected
            </h2>

            <p className="mt-1 text-sm leading-6 text-slate-500">
              Review the devices currently signed in to your
              DevFlow account. Revoke any session you don't
              recognize.
            </p>
          </div>
        </div>
      </section>

      {/* Active Sessions */}
      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        {/* Section Header */}
        <div className="border-b border-slate-200 px-6 py-5">
          <div className="flex items-center justify-between gap-4">
            <div>
              <h2 className="text-base font-semibold text-slate-900">
                Active Sessions
              </h2>

              <p className="mt-1 text-sm text-slate-500">
                Review devices currently signed in to your
                DevFlow account.
              </p>
            </div>

            <div className="hidden rounded-full bg-slate-50 px-3 py-1.5 text-xs font-medium text-slate-500 sm:block">
              Session Management
            </div>
          </div>
        </div>

        {/* Sessions */}
        <div className="p-6">
          <SessionsList />
        </div>
      </section>
    </div>
  );
}