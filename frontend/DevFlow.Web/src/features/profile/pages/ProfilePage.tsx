import {
  Mail,
  Shield,
  User,
} from "lucide-react";

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
        <div className="flex items-center gap-3 text-sm text-slate-500">
          <div className="h-5 w-5 animate-spin rounded-full border-2 border-slate-200 border-t-blue-600" />
          Loading profile...
        </div>
      </div>
    );
  }

  if (isError || !profile) {
    return (
      <div className="flex min-h-[50vh] items-center justify-center">
        <div className="rounded-xl border border-red-200 bg-white px-6 py-5 text-center shadow-sm">
          <p className="text-sm font-medium text-red-600">
            Unable to load your profile.
          </p>

          <p className="mt-1 text-xs text-slate-500">
            Please try refreshing the page.
          </p>
        </div>
      </div>
    );
  }

  const initials =
    profile.firstName?.charAt(0).toUpperCase() ?? "U";

  return (
    <div className="mx-auto w-full max-w-5xl space-y-6">

      {/* Page Header */}
      <div>
        <p className="text-sm font-medium text-blue-600">
          Account
        </p>

        <h1 className="mt-1 text-2xl font-semibold tracking-tight text-slate-900">
          Profile
        </h1>

        <p className="mt-1 text-sm text-slate-500">
          Manage your account information and personal details.
        </p>
      </div>

      {/* Profile Summary */}
      <section className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">

        <div className="p-6">

          <div className="flex flex-col gap-5 sm:flex-row sm:items-center">

            {/* Avatar */}
            <div className="flex h-20 w-20 shrink-0 items-center justify-center rounded-full bg-blue-600 text-2xl font-semibold text-white shadow-sm">
              {initials}
            </div>

            {/* User Information */}
            <div className="min-w-0">

              <h2 className="text-xl font-semibold tracking-tight text-slate-900">
                {profile.fullName}
              </h2>

              <p className="mt-1 text-sm text-slate-500">
                {profile.email}
              </p>

              <div className="mt-3">
                <span className="inline-flex rounded-full bg-blue-50 px-3 py-1 text-xs font-medium text-blue-700">
                  {profile.role}
                </span>
              </div>

            </div>

          </div>

        </div>
      </section>

      {/* Account Information */}
      <section className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">

        {/* Header */}
        <div className="border-b border-slate-100 px-6 py-5">

          <h2 className="text-sm font-semibold text-slate-900">
            Account Information
          </h2>

          <p className="mt-1 text-xs text-slate-500">
            Your basic DevFlow account information.
          </p>

        </div>

        {/* Information */}
        <div className="grid gap-0 md:grid-cols-2">

          {/* Full Name */}
          <ProfileField
            icon={User}
            label="Full Name"
            value={profile.fullName}
          />

          {/* Email */}
          <ProfileField
            icon={Mail}
            label="Email Address"
            value={profile.email}
          />

          {/* Role */}
          <ProfileField
            icon={Shield}
            label="Role"
            value={profile.role}
          />

        </div>

      </section>

    </div>
  );
}

/* -------------------------------------------------------------------------- */
/* Profile Field                                                              */
/* -------------------------------------------------------------------------- */

function ProfileField({
  icon: Icon,
  label,
  value,
}: {
  icon: React.ComponentType<{
    className?: string;
  }>;
  label: string;
  value: string;
}) {
  return (
    <div className="flex items-start gap-4 border-b border-slate-100 p-6 last:border-b-0 md:border-r md:[&:nth-child(2n)]:border-r-0">

      <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-slate-100">
        <Icon className="h-5 w-5 text-slate-600" />
      </div>

      <div className="min-w-0">

        <p className="text-xs font-medium uppercase tracking-wide text-slate-400">
          {label}
        </p>

        <p className="mt-1 truncate text-sm font-medium text-slate-900">
          {value}
        </p>

      </div>

    </div>
  );
}