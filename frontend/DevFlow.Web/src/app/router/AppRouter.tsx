import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";

import { AppLayout } from "@/app/layout/AppLayout";

import { LoginPage } from "@/features/auth/pages/LoginPage";
import { DashboardPage } from "@/features/dashboard/pages/DashboardPage";
import { ProjectsPage } from "@/features/projects/pages/ProjectsPage";
import { ProjectDetailPage } from "@/features/projects/pages/ProjectDetailPage";

import { ProfilePage } from "@/features/profile/pages/ProfilePage";
import { SecurityPage } from "@/features/security/pages/SecurityPage";

import { ProtectedRoute } from "./ProtectedRoute";

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
              path="/projects/:projectId"
              element={<ProjectDetailPage />}
            />

            {/* ============================================================ */}
            {/* WORK                                                         */}
            {/* ============================================================ */}

            <Route
              path="/work"
              element={
                <div>Work</div>
              }
            />

            {/* ============================================================ */}
            {/* ACTIVITY                                                     */}
            {/* ============================================================ */}

            <Route
              path="/activity"
              element={
                <div>Activity</div>
              }
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
              element={
                <div>Settings</div>
              }
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