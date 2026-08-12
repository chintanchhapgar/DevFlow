import { Mail, Shield, User } from "lucide-react";
import { useProfile } from "@/features/auth/hooks/use-profile";

export function ProfilePage() {
  const {
    data: profile,
    isLoading,
    isError,
  } = useProfile();

  if (isLoading) {
    return (
      <div className="flex min-h-[50vh] items-center justify-center">
        <span className="text-sm text-slate-400">
          Loading profile...
        </span>
      </div>
    );
  }

  if (isError || !profile) {
    return (
      <div className="flex min-h-[50vh] items-center justify-center">
        <p className="text-sm text-red-400">
          Unable to load your profile.
        </p>
      </div>
    );
  }

  const initials =
    profile.firstName?.charAt(0).toUpperCase() ?? "U";

  return (
    <div className="mx-auto w-full max-w-5xl space-y-8">
      {/* Page Header */}
      <div>
        <h1 className="text-3xl font-bold tracking-tight">
          Profile
        </h1>

        <p className="mt-2 text-slate-400">
          Manage your account information.
        </p>
      </div>

      {/* Profile Summary */}
      <section className="rounded-2xl border border-white/10 bg-white/5">
        <div className="flex flex-col gap-6 p-6 sm:flex-row sm:items-center">
          <div className="flex h-20 w-20 shrink-0 items-center justify-center rounded-full bg-primary text-2xl font-bold text-primary-foreground">
            {initials}
          </div>

          <div>
            <h2 className="text-2xl font-semibold text-white">
              {profile.fullName}
            </h2>

            <p className="mt-1 text-sm text-slate-400">
              {profile.email}
            </p>

            <span className="mt-3 inline-flex rounded-full bg-primary/10 px-3 py-1 text-xs font-medium text-primary">
              {profile.role}
            </span>
          </div>
        </div>
      </section>

      {/* Account Information */}
      <section className="rounded-2xl border border-white/10 bg-white/5">
        <div className="border-b border-white/10 px-6 py-4">
          <h2 className="font-semibold text-white">
            Account Information
          </h2>

          <p className="mt-1 text-sm text-slate-400">
            Your basic DevFlow account information.
          </p>
        </div>

        <div className="grid gap-6 p-6 md:grid-cols-2">
          {/* Name */}
          <div className="flex items-start gap-4">
            <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-white/5">
              <User className="h-5 w-5 text-slate-400" />
            </div>

            <div>
              <p className="text-xs font-medium uppercase tracking-wide text-slate-500">
                Full Name
              </p>

              <p className="mt-1 text-sm text-white">
                {profile.fullName}
              </p>
            </div>
          </div>

          {/* Email */}
          <div className="flex items-start gap-4">
            <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-white/5">
              <Mail className="h-5 w-5 text-slate-400" />
            </div>

            <div>
              <p className="text-xs font-medium uppercase tracking-wide text-slate-500">
                Email Address
              </p>

              <p className="mt-1 text-sm text-white">
                {profile.email}
              </p>
            </div>
          </div>

          {/* Role */}
          <div className="flex items-start gap-4">
            <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-white/5">
              <Shield className="h-5 w-5 text-slate-400" />
            </div>

            <div>
              <p className="text-xs font-medium uppercase tracking-wide text-slate-500">
                Role
              </p>

              <p className="mt-1 text-sm text-white">
                {profile.role}
              </p>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}