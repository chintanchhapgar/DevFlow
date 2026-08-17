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
import { RegisterPage } from "@/features/auth/pages/RegisterPage";
import { ForgotPasswordPage } from "@/features/auth/pages/ForgotPasswordPage";
import { ResetPasswordPage } from "@/features/auth/pages/ResetPasswordPage";
import { VerifyEmailPage } from "@/features/auth/pages/VerifyEmailPage";
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
import { SprintReportsPage } from "@/features/reports/pages/SprintReportsPage";
import { WorkItemDetailPage } from "@/features/projects/pages/WorkItemDetailPage";
import { UsersPage } from "@/features/users/pages/UsersPage";
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
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/forgot-password" element={<ForgotPasswordPage />} />
        <Route path="/reset-password" element={<ResetPasswordPage />} />
        <Route path="/verify-email" element={<VerifyEmailPage />} />
        <Route
          path="/invitations/respond"
          element={<InvitationResponsePage />}
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
              path="/projects/:projectId"
              element={<ProjectDetailPage />}
            />

            <Route
              path="/projects/:projectId/work-items/:workItemId"
              element={<WorkItemDetailPage />}
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

            <Route
              path="/reports/sprints"
              element={<SprintReportsPage />}
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

            <Route
              path="/users"
              element={<UsersPage />}
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
