import { useState, type FormEvent } from "react";
import { Link, useSearchParams } from "react-router-dom";
import axios from "axios";
import { CheckCircle2, Loader2 } from "lucide-react";
import { resetPassword } from "@/features/auth/api/auth-api";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { AuthFormField } from "./AuthFormField";
import { AuthPageLayout } from "./AuthPageLayout";

export function ResetPasswordPage() {
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token") ?? "";
  const [password, setPassword] = useState(""); const [confirmPassword, setConfirmPassword] = useState(""); const [error, setError] = useState(""); const [complete, setComplete] = useState(false); const [isSubmitting, setIsSubmitting] = useState(false);
  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!token) { setError("This reset link is invalid or incomplete."); return; }
    if (password !== confirmPassword) { setError("Passwords do not match."); return; }
    setError(""); setIsSubmitting(true);
    try { await resetPassword(token, password); setComplete(true); }
    catch (caughtError) { setError(axios.isAxiosError(caughtError) && caughtError.response?.data?.message ? String(caughtError.response.data.message) : "Unable to reset your password. The link may have expired."); }
    finally { setIsSubmitting(false); }
  }
  return <AuthPageLayout>
    <div className="mb-8"><h1 className="text-3xl font-bold tracking-tight">Choose a new password</h1><p className="mt-2 text-sm leading-6 text-slate-500">Set a strong password to secure your DevFlow account.</p></div>
    <Card className="border-slate-200 shadow-sm"><CardHeader className="border-b border-slate-100 px-6 py-5"><CardTitle className="text-base">New password</CardTitle><CardDescription>Use at least 8 characters.</CardDescription></CardHeader><CardContent className="px-6 py-6">
      {complete ? <div className="space-y-4 text-center"><CheckCircle2 className="mx-auto h-10 w-10 text-emerald-600" /><p className="text-sm leading-6 text-slate-600">Your password has been reset. You can now sign in with your new password.</p><Button asChild className="w-full bg-[#456b9a] hover:bg-[#3d608b]"><Link to="/login">Sign in</Link></Button></div> : <form onSubmit={handleSubmit} className="space-y-5">{error && <div role="alert" className="rounded-lg border border-red-200 bg-red-50 px-3.5 py-3 text-sm text-red-700">{error}</div>}<AuthFormField id="password" label="New password" type="password" value={password} onChange={(event) => setPassword(event.target.value)} autoComplete="new-password" hint="At least 8 characters" /><AuthFormField id="confirmPassword" label="Confirm new password" type="password" value={confirmPassword} onChange={(event) => setConfirmPassword(event.target.value)} autoComplete="new-password" /><Button type="submit" disabled={isSubmitting || !token} className="h-11 w-full bg-[#456b9a] hover:bg-[#3d608b]">{isSubmitting ? <><Loader2 className="mr-2 h-4 w-4 animate-spin" />Resetting password...</> : "Reset password"}</Button>{!token && <p className="text-center text-xs text-red-600">Missing reset token. Request a new reset link.</p>}</form>}
    </CardContent></Card>
    <p className="mt-6 text-center text-sm text-slate-500">Remembered your password? <Link to="/login" className="font-medium text-[#456b9a] hover:underline">Sign in</Link></p>
  </AuthPageLayout>;
}
