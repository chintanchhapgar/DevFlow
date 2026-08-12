import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { ArrowRight, Loader2 } from "lucide-react";
import axios from "axios";

import { login } from "@/features/auth/api/auth-api";
import { authStorage } from "@/features/auth/auth-storage";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

export function LoginPage() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState("");

  async function handleSubmit(
    event: React.FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    setError("");
    setIsSubmitting(true);

    try {
      const result = await login({
        email,
        password,
      });

      authStorage.setTokens(
        result.accessToken,
        result.refreshToken,
      );

      navigate("/", {
        replace: true,
      });
    } catch (error) {
      if (
        axios.isAxiosError(error) &&
        error.response?.data?.message
      ) {
        setError(error.response.data.message);
      } else {
        setError("Unable to sign in. Please try again.");
      }
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="min-h-screen bg-slate-50">

      {/* Main */}
      <main className="flex min-h-screen items-center justify-center px-4 py-12">

        <div className="w-full max-w-md">

          {/* Brand */}
          <div className="mb-8 text-center">

            <Link
              to="/"
              className="inline-flex items-center gap-2"
            >
              <div className="flex h-9 w-9 items-center justify-center rounded-lg bg-blue-600 text-sm font-bold text-white shadow-sm">
                ◆
              </div>

              <span className="text-lg font-bold tracking-tight text-slate-900">
                DEVFLOW
              </span>
            </Link>

            <p className="mt-3 text-sm text-slate-500">
              Modern project management for productive teams.
            </p>

          </div>

          {/* Login Card */}
          <Card className="border-slate-200 bg-white shadow-sm">

            <CardHeader className="space-y-1 pb-5">

              <CardTitle className="text-xl font-semibold text-slate-900">
                Welcome back
              </CardTitle>

              <CardDescription className="text-slate-500">
                Sign in to continue to your workspace.
              </CardDescription>

            </CardHeader>

            <CardContent>

              <form
                onSubmit={handleSubmit}
                className="space-y-5"
              >

                {/* Error */}
                {error && (
                  <div className="rounded-lg border border-red-200 bg-red-50 px-3 py-2.5 text-sm text-red-700">
                    {error}
                  </div>
                )}

                {/* Email */}
                <div className="space-y-2">

                  <Label
                    htmlFor="email"
                    className="text-sm font-medium text-slate-700"
                  >
                    Email
                  </Label>

                  <Input
                    id="email"
                    type="email"
                    placeholder="you@example.com"
                    value={email}
                    onChange={(event) =>
                      setEmail(event.target.value)
                    }
                    required
                    autoComplete="email"
                    disabled={isSubmitting}
                    className="h-10 border-slate-200 bg-white text-slate-900 placeholder:text-slate-400 focus-visible:ring-blue-500"
                  />

                </div>

                {/* Password */}
                <div className="space-y-2">

                  <div className="flex items-center justify-between">

                    <Label
                      htmlFor="password"
                      className="text-sm font-medium text-slate-700"
                    >
                      Password
                    </Label>

                    <Link
                      to="/forgot-password"
                      className="text-xs font-medium text-blue-600 transition hover:text-blue-700 hover:underline"
                    >
                      Forgot password?
                    </Link>

                  </div>

                  <Input
                    id="password"
                    type="password"
                    value={password}
                    onChange={(event) =>
                      setPassword(event.target.value)
                    }
                    required
                    autoComplete="current-password"
                    disabled={isSubmitting}
                    className="h-10 border-slate-200 bg-white text-slate-900 focus-visible:ring-blue-500"
                  />

                </div>

                {/* Submit */}
                <Button
                  type="submit"
                  disabled={isSubmitting}
                  className="
                    h-10
                    w-full
                    bg-blue-600
                    font-medium
                    text-white
                    shadow-sm
                    transition
                    hover:bg-blue-700
                    focus-visible:ring-blue-500
                  "
                >
                  {isSubmitting ? (
                    <>
                      <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                      Signing in...
                    </>
                  ) : (
                    <>
                      Sign in
                      <ArrowRight className="ml-2 h-4 w-4" />
                    </>
                  )}
                </Button>

                {/* Register */}
                <p className="pt-1 text-center text-sm text-slate-500">

                  Don't have an account?{" "}

                  <Link
                    to="/register"
                    className="font-medium text-blue-600 transition hover:text-blue-700 hover:underline"
                  >
                    Create account
                  </Link>

                </p>

              </form>

            </CardContent>

          </Card>

          {/* Footer */}
          <p className="mt-6 text-center text-xs text-slate-400">
            © 2026 DevFlow. All rights reserved.
          </p>

        </div>

      </main>
    </div>
  );
}