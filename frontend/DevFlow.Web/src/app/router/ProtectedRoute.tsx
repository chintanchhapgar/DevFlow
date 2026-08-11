import { Navigate, Outlet } from "react-router-dom";

import { authStorage } from "@/features/auth/auth-storage";

export function ProtectedRoute() {
  const accessToken = authStorage.getAccessToken();

  if (!accessToken) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
}