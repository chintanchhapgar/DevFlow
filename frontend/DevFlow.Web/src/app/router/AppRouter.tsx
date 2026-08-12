import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";

import { AppLayout } from "@/app/layout/AppLayout";
import { LoginPage } from "@/features/auth/pages/LoginPage";
import { DashboardPage } from "@/features/dashboard/pages/DashboardPage";
import { ProtectedRoute } from "./ProtectedRoute";
import { ProfilePage } from "@/features/profile/pages/ProfilePage";
import { SecurityPage } from "@/features/security/pages/SecurityPage";
export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/login"
          element={<LoginPage />}
        />

        <Route element={<ProtectedRoute />}>
          <Route element={<AppLayout />}>
            <Route
              path="/"
              element={<DashboardPage />}
            />

            <Route
              path="/projects"
              element={
                <div>Projects</div>
              }
            />

            <Route
              path="/work"
              element={
                <div>Work</div>
              }
            />

            <Route
              path="/activity"
              element={
                <div>Activity</div>
              }
            />

            <Route
              path="/profile"
              element={<ProfilePage />}
            />

            <Route
              path="/security"
              element={<SecurityPage />}
            />

            <Route
              path="/settings"
              element={
                <div>Settings</div>
              }
            />
          </Route>
        </Route>

        <Route
          path="*"
          element={
            <Navigate
              to="/"
              replace
            />
          }
        />
      </Routes>
    </BrowserRouter>
  );
}