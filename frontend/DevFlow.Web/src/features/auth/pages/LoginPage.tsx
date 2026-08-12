import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import {
  ArrowRight,
  CheckCircle2,
  Loader2,
  ShieldCheck,
} from "lucide-react";
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

  const [isSubmitting, setIsSubmitting] =
    useState(false);

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
        setError("Unable to sign in.");
      }
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="min-h-screen bg-slate-50 text-slate-900">
      <div className="grid min-h-screen lg:grid-cols-[1.05fr_0.95fr]">
        {/* Left Brand Panel */}
        <div className="relative hidden overflow-hidden bg-[#eef3f8] lg:flex">
          {/* Decorative shapes */}
          <div className="absolute -left-24 -top-24 h-72 w-72 rounded-full bg-[#456b9a]/10" />

          <div className="absolute -bottom-32 -right-20 h-96 w-96 rounded-full bg-[#456b9a]/10" />

          <div className="relative flex w-full flex-col justify-between p-12 xl:p-16">
            {/* Brand */}
            <div className="flex items-center gap-3">
              <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-[#456b9a] text-lg font-bold text-white shadow-sm">
                ◆
              </div>

              <div>
                <div className="text-lg font-bold tracking-tight text-slate-800">
                  DEVFLOW
                </div>

                <div className="text-xs text-slate-500">
                  Project Management
                </div>
              </div>
            </div>

            {/* Main message */}
            <div className="max-w-lg">
              <div className="mb-5 inline-flex items-center gap-2 rounded-full border border-slate-200 bg-white px-3 py-1.5 text-xs font-medium text-slate-600 shadow-sm">
                <span className="h-1.5 w-1.5 rounded-full bg-emerald-500" />

                Built for modern teams
              </div>

              <h1 className="text-4xl font-bold leading-tight tracking-tight text-slate-900 xl:text-5xl">
                Plan work.
                <br />
                Build together.
                <br />
                <span className="text-[#456b9a]">
                  Ship better.
                </span>
              </h1>

              <p className="mt-6 max-w-md text-base leading-7 text-slate-500">
                DevFlow brings projects, work, activity,
                and team collaboration together in one
                focused workspace.
              </p>

              <div className="mt-8 space-y-3">
                {[
                  "Organize projects and work",
                  "Track team activity",
                  "Keep your workspace secure",
                ].map((item) => (
                  <div
                    key={item}
                    className="flex items-center gap-3 text-sm text-slate-600"
                  >
                    <CheckCircle2 className="h-4 w-4 text-[#456b9a]" />

                    <span>{item}</span>
                  </div>
                ))}
              </div>
            </div>

            {/* Footer */}
            <p className="text-xs text-slate-400">
              © {new Date().getFullYear()} DevFlow
            </p>
          </div>
        </div>

        {/* Login Panel */}
        <div className="flex min-h-screen items-center justify-center bg-white px-5 py-10 sm:px-8">
          <div className="w-full max-w-md">
            {/* Mobile Brand */}
            <div className="mb-10 flex items-center justify-center gap-3 lg:hidden">
              <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-[#456b9a] text-lg font-bold text-white shadow-sm">
                ◆
              </div>

              <div>
                <div className="text-lg font-bold tracking-tight text-slate-800">
                  DEVFLOW
                </div>

                <div className="text-xs text-slate-400">
                  Project Management
                </div>
              </div>
            </div>

            {/* Header */}
            <div className="mb-8">
              <h2 className="text-3xl font-bold tracking-tight text-slate-900">
                Welcome back
              </h2>

              <p className="mt-2 text-sm leading-6 text-slate-500">
                Sign in to continue to your DevFlow
                workspace.
              </p>
            </div>

            <Card className="border-slate-200 bg-white shadow-sm">
              <CardHeader className="space-y-1 border-b border-slate-100 px-6 py-5">
                <CardTitle className="text-base font-semibold text-slate-800">
                  Sign in
                </CardTitle>

                <CardDescription className="text-slate-500">
                  Enter your account credentials below.
                </CardDescription>
              </CardHeader>

              <CardContent className="px-6 py-6">
                <form
                  onSubmit={handleSubmit}
                  className="space-y-5"
                >
                  {/* Error */}
                  {error && (
                    <div
                      role="alert"
                      className="
                        rounded-lg
                        border
                        border-red-200
                        bg-red-50
                        px-3.5
                        py-3
                        text-sm
                        text-red-700
                      "
                    >
                      {error}
                    </div>
                  )}

                  {/* Email */}
                  <div className="space-y-2">
                    <Label
                      htmlFor="email"
                      className="text-slate-700"
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
                      className="
                        h-11
                        border-slate-200
                        bg-white
                        text-slate-900
                        placeholder:text-slate-400
                        focus-visible:border-[#7890aa]
                        focus-visible:ring-[#456b9a]/20
                      "
                    />
                  </div>

                  {/* Password */}
                  <div className="space-y-2">
                    <div className="flex items-center justify-between">
                      <Label
                        htmlFor="password"
                        className="text-slate-700"
                      >
                        Password
                      </Label>

                      <Link
                        to="/forgot-password"
                        className="
                          text-xs
                          font-medium
                          text-[#456b9a]
                          transition-colors
                          hover:text-[#36597f]
                          hover:underline
                        "
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
                      className="
                        h-11
                        border-slate-200
                        bg-white
                        text-slate-900
                        focus-visible:border-[#7890aa]
                        focus-visible:ring-[#456b9a]/20
                      "
                    />
                  </div>

                  {/* Submit */}
                  <Button
                    type="submit"
                    disabled={isSubmitting}
                    className="
                      h-11
                      w-full
                      bg-[#456b9a]
                      font-medium
                      text-white
                      shadow-sm
                      transition-colors
                      hover:bg-[#3d608b]
                      focus-visible:ring-[#456b9a]/25
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

                  {/* Security */}
                  <div className="flex items-center justify-center gap-2 pt-1 text-xs text-slate-400">
                    <ShieldCheck className="h-3.5 w-3.5" />

                    <span>
                      Your connection is secured
                    </span>
                  </div>
                </form>
              </CardContent>
            </Card>

            {/* Register */}
            <p className="mt-6 text-center text-sm text-slate-500">
              Don't have an account?{" "}
              <Link
                to="/register"
                className="
                  font-medium
                  text-[#456b9a]
                  hover:text-[#36597f]
                  hover:underline
                "
              >
                Create account
              </Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}