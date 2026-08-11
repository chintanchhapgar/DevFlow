import { useProfile } from "@/features/auth/hooks/use-profile";

export function DashboardPage() {
  const {
    data: profile,
    isLoading,
    isError,
  } = useProfile();

  if (isLoading) {
    return (
      <div className="flex min-h-[50vh] items-center justify-center">
        <span className="text-sm text-slate-400">
          Loading...
        </span>
      </div>
    );
  }

  if (isError || !profile) {
    return (
      <div className="flex min-h-[50vh] items-center justify-center">
        <p className="text-sm text-slate-400">
          Unable to load your profile.
        </p>
      </div>
    );
  }

  return (
    <div className="mx-auto w-full max-w-7xl">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">
          Welcome, {profile.firstName}
        </h1>

        <p className="mt-2 text-slate-400">
          You're successfully authenticated.
        </p>
      </div>

      <div className="mt-8 rounded-xl border border-white/10 bg-white/5 p-6">
        <h2 className="text-lg font-semibold">
          Profile
        </h2>

        <div className="mt-4 space-y-3 text-sm">
          <p>
            <span className="text-slate-400">
              Name:
            </span>{" "}
            <span className="text-white">
              {profile.fullName}
            </span>
          </p>

          <p>
            <span className="text-slate-400">
              Email:
            </span>{" "}
            <span className="text-white">
              {profile.email}
            </span>
          </p>

          <p>
            <span className="text-slate-400">
              Role:
            </span>{" "}
            <span className="text-white">
              {profile.role}
            </span>
          </p>
        </div>
      </div>
    </div>
  );
}