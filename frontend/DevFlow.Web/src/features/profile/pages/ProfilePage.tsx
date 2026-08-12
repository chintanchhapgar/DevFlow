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
          <div className="h-4 w-4 animate-spin rounded-full border-2 border-slate-200 border-t-[#456b9a]" />
          Loading profile...
        </div>
      </div>
    );
  }

  if (isError || !profile) {
    return (
      <div className="mx-auto flex min-h-[50vh] w-full max-w-5xl items-center justify-center">
        <div className="rounded-xl border border-red-200 bg-red-50 px-5 py-4 text-sm text-red-700">
          Unable to load your profile.
        </div>
      </div>
    );
  }

  const initials =
    profile.firstName?.charAt(0).toUpperCase() ?? "U";

  return (
    <div className="mx-auto w-full max-w-5xl space-y-8">
      {/* Page Header */}
      <div>
        <h1 className="text-2xl font-bold tracking-tight text-slate-900 sm:text-3xl">
          Profile
        </h1>

        <p className="mt-2 text-sm text-slate-500">
          Manage your account information and personal
          details.
        </p>
      </div>

      {/* Profile Summary */}
      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <div className="h-24 bg-[#eef3f8]" />

        <div className="px-6 pb-6">
          <div className="-mt-10 flex flex-col gap-5 sm:flex-row sm:items-end">
            {/* Avatar */}
            <div
              className="
                flex
                h-20
                w-20
                shrink-0
                items-center
                justify-center
                rounded-2xl
                border-4
                border-white
                bg-[#456b9a]
                text-2xl
                font-bold
                text-white
                shadow-sm
              "
            >
              {initials}
            </div>

            {/* User */}
            <div className="min-w-0 flex-1 pb-1">
              <h2 className="truncate text-xl font-semibold text-slate-900">
                {profile.fullName}
              </h2>

              <p className="mt-1 truncate text-sm text-slate-500">
                {profile.email}
              </p>
            </div>

            {/* Role */}
            <div className="pb-1">
              <span
                className="
                  inline-flex
                  items-center
                  rounded-full
                  border
                  border-slate-200
                  bg-slate-50
                  px-3
                  py-1.5
                  text-xs
                  font-medium
                  text-slate-600
                "
              >
                {profile.role}
              </span>
            </div>
          </div>
        </div>
      </section>

      {/* Account Information */}
      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        {/* Section Header */}
        <div className="border-b border-slate-200 px-6 py-5">
          <h2 className="text-base font-semibold text-slate-900">
            Account Information
          </h2>

          <p className="mt-1 text-sm text-slate-500">
            Your basic DevFlow account information.
          </p>
        </div>

        {/* Information */}
        <div className="grid gap-0 md:grid-cols-2">
          {/* Full Name */}
          <div className="border-b border-slate-200 p-6 md:border-r">
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
                  bg-[#eef3f8]
                "
              >
                <User className="h-5 w-5 text-[#456b9a]" />
              </div>

              <div className="min-w-0">
                <p className="text-xs font-semibold uppercase tracking-wide text-slate-400">
                  Full Name
                </p>

                <p className="mt-1.5 truncate text-sm font-medium text-slate-800">
                  {profile.fullName}
                </p>
              </div>
            </div>
          </div>

          {/* Email */}
          <div className="border-b border-slate-200 p-6">
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
                  bg-[#eef3f8]
                "
              >
                <Mail className="h-5 w-5 text-[#456b9a]" />
              </div>

              <div className="min-w-0">
                <p className="text-xs font-semibold uppercase tracking-wide text-slate-400">
                  Email Address
                </p>

                <p className="mt-1.5 truncate text-sm font-medium text-slate-800">
                  {profile.email}
                </p>
              </div>
            </div>
          </div>

          {/* Role */}
          <div className="p-6 md:border-r">
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
                  bg-[#eef3f8]
                "
              >
                <Shield className="h-5 w-5 text-[#456b9a]" />
              </div>

              <div className="min-w-0">
                <p className="text-xs font-semibold uppercase tracking-wide text-slate-400">
                  Role
                </p>

                <p className="mt-1.5 text-sm font-medium text-slate-800">
                  {profile.role}
                </p>
              </div>
            </div>
          </div>

          {/* Account Status */}
          <div className="border-t border-slate-200 p-6 md:border-t-0">
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
                <span className="h-2.5 w-2.5 rounded-full bg-emerald-500" />
              </div>

              <div>
                <p className="text-xs font-semibold uppercase tracking-wide text-slate-400">
                  Account Status
                </p>

                <div className="mt-1.5 flex items-center gap-2">
                  <span className="text-sm font-medium text-slate-800">
                    Active
                  </span>

                  <span className="rounded-full bg-emerald-50 px-2 py-0.5 text-[11px] font-medium text-emerald-700">
                    Verified
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Security Shortcut */}
      <section className="flex flex-col gap-4 rounded-2xl border border-slate-200 bg-white p-6 shadow-sm sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h2 className="text-sm font-semibold text-slate-900">
            Keep your account secure
          </h2>

          <p className="mt-1 text-sm text-slate-500">
            Review active sessions and manage your
            account security.
          </p>
        </div>

        <a
          href="/security"
          className="
            inline-flex
            shrink-0
            items-center
            justify-center
            rounded-lg
            border
            border-slate-200
            bg-white
            px-4
            py-2
            text-sm
            font-medium
            text-slate-700
            shadow-sm
            transition-colors
            hover:bg-slate-50
            hover:text-[#456b9a]
          "
        >
          Security settings
        </a>
      </section>
    </div>
  );
}