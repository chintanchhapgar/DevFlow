import { useProfile } from "@/features/auth/hooks/use-profile";

export function DashboardPage() {
  const {
    data: profile,
    isLoading,
    isError,
  } = useProfile();

  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        Loading...
      </div>
    );
  }

  if (isError || !profile) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <p>Unable to load your profile.</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-slate-950 text-white">
      <header className="border-b border-white/10">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-4">
          <h1 className="text-xl font-bold">
            DevFlow
          </h1>

          <span className="text-sm text-slate-400">
            {profile.email}
          </span>
        </div>
      </header>

      <main className="mx-auto max-w-7xl px-6 py-10">
        <h2 className="text-3xl font-bold">
          Welcome, {profile.firstName}
        </h2>

        <p className="mt-2 text-slate-400">
          You're successfully authenticated.
        </p>

        <div className="mt-8 rounded-xl border border-white/10 bg-white/5 p-6">
          <h3 className="text-lg font-semibold">
            Profile
          </h3>

          <div className="mt-4 space-y-2 text-sm">
            <p>
              <span className="text-slate-400">
                Name:
              </span>{" "}
              {profile.fullName}
            </p>

            <p>
              <span className="text-slate-400">
                Email:
              </span>{" "}
              {profile.email}
            </p>

            <p>
              <span className="text-slate-400">
                Role:
              </span>{" "}
              {profile.role}
            </p>
          </div>
        </div>
      </main>
    </div>
  );
}