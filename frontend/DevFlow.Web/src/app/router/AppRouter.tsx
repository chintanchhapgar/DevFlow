import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";

import { AppLayout } from "@/app/layout/AppLayout";
import { WorkPage } from "@/features/projects/pages/WorkPage";
import { ActivityPage } from "@/features/activity/pages/ActivityPage";
import { LoginPage } from "@/features/auth/pages/LoginPage";
import { DashboardPage } from "@/features/dashboard/pages/DashboardPage";
import { ProjectsPage } from "@/features/projects/pages/ProjectsPage";
import { ProjectDetailPage } from "@/features/projects/pages/ProjectDetailPage";
import { InvitationResponsePage } from "@/features/projects/pages/InvitationResponsePage";
import { ProfilePage } from "@/features/profile/pages/ProfilePage";
import { SecurityPage } from "@/features/security/pages/SecurityPage";
import { SettingsPage } from "@/features/settings/pages/SettingsPage";
import { ProtectedRoute } from "./ProtectedRoute";
import { MyTimePage } from "@/features/time/pages/MyTimePage";
import { ReportsPage } from "@/features/reports/pages/ReportsPage";

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        {/* ================================================================== */}
        {/* PUBLIC ROUTES                                                     */}
        {/* ================================================================== */}

        <Route
          path="/login"
          element={<LoginPage />}
        />

        {/* ================================================================== */}
        {/* PROTECTED ROUTES                                                  */}
        {/* ================================================================== */}

        <Route element={<ProtectedRoute />}>
          <Route element={<AppLayout />}>

            {/* Dashboard */}
            <Route
              path="/"
              element={<DashboardPage />}
            />

            {/* ============================================================ */}
            {/* PROJECTS                                                      */}
            {/* ============================================================ */}

            <Route
              path="/projects"
              element={<ProjectsPage />}
            />

            <Route
              path="/invitations/respond"
              element={<InvitationResponsePage />}
            />

            <Route
              path="/projects/:projectId"
              element={<ProjectDetailPage />}
            />

            {/* ============================================================ */}
            {/* WORK                                                         */}
            {/* ============================================================ */}

            <Route
              path="/work"
              element={<WorkPage />}
            />

            <Route
              path="/time"
              element={<MyTimePage />}
            />

            <Route
              path="/reports"
              element={<ReportsPage />}
            />
            {/* ============================================================ */}
            {/* ACTIVITY                                                     */}
            {/* ============================================================ */}

            <Route
              path="/activity"
              element={<ActivityPage />}
            />

            {/* ============================================================ */}
            {/* PROFILE                                                      */}
            {/* ============================================================ */}

            <Route
              path="/profile"
              element={<ProfilePage />}
            />

            {/* ============================================================ */}
            {/* SECURITY                                                     */}
            {/* ============================================================ */}

            <Route
              path="/security"
              element={<SecurityPage />}
            />

            {/* ============================================================ */}
            {/* SETTINGS                                                     */}
            {/* ============================================================ */}

            <Route
              path="/settings"
              element={<SettingsPage />}
            />

          </Route>
        </Route>

        {/* ================================================================== */}
        {/* FALLBACK                                                          */}
        {/* ================================================================== */}

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
