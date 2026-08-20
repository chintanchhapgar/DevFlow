import { Navigate, Outlet } from "react-router-dom";

import { useProfile } from "@/features/auth/hooks/use-profile";
import { canViewReports } from "@/features/auth/user-roles";

export function ReportsRoute() {
  const profileQuery = useProfile();

  if (profileQuery.isLoading) {
    return <div className="flex min-h-[50vh] items-center justify-center text-sm text-slate-500">Checking report access…</div>;
  }

  return canViewReports(profileQuery.data?.role)
    ? <Outlet />
    : <Navigate to="/dashboard" replace />;
}
