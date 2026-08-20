import { Navigate, Outlet, useLocation } from "react-router-dom";

import { authStorage } from "@/features/auth/auth-storage";

export function ProtectedRoute() {
  const accessToken = authStorage.getAccessToken();
  const location = useLocation();

  if (!accessToken) {
    const returnTo = `${location.pathname}${location.search}${location.hash}`;
    return <Navigate to={`/login?returnTo=${encodeURIComponent(returnTo)}`} replace />;
  }

  return <Outlet />;
}
