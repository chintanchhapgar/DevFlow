import { Shield } from "lucide-react";

import { SessionsList } from "../components/SessionsList";

export function SecurityPage() {
  return (
    <div className="mx-auto w-full max-w-5xl space-y-8">
      <div>
        <div className="flex items-center gap-3">
          <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10">
            <Shield className="h-5 w-5 text-primary" />
          </div>

          <div>
            <h1 className="text-3xl font-bold tracking-tight">
              Security
            </h1>

            <p className="mt-1 text-slate-400">
              Manage your account security and active sessions.
            </p>
          </div>
        </div>
      </div>

      <section className="rounded-2xl border border-white/10 bg-white/5">
        <div className="border-b border-white/10 px-6 py-5">
          <h2 className="text-lg font-semibold text-white">
            Active Sessions
          </h2>

          <p className="mt-1 text-sm text-slate-400">
            Review devices currently signed in to your
            DevFlow account.
          </p>
        </div>

        <div className="p-6">
          <SessionsList />
        </div>
      </section>
    </div>
  );
}