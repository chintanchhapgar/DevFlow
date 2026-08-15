import { useState, type FormEvent } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import { ArrowLeft, Loader2, MailCheck } from "lucide-react";
import { requestPasswordReset } from "@/features/auth/api/auth-api";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { AuthFormField } from "./AuthFormField";
import { AuthPageLayout } from "./AuthPageLayout";

export function ForgotPasswordPage() {
  const [email, setEmail] = useState(""); const [error, setError] = useState(""); const [sent, setSent] = useState(false); const [isSubmitting, setIsSubmitting] = useState(false);
  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault(); setError(""); setIsSubmitting(true);
    try { await requestPasswordReset(email); setSent(true); }
    catch (caughtError) { setError(axios.isAxiosError(caughtError) && caughtError.response?.data?.message ? String(caughtError.response.data.message) : "Unable to request a password reset."); }
    finally { setIsSubmitting(false); }
  }
  return <AuthPageLayout>
    <div className="mb-8"><h1 className="text-3xl font-bold tracking-tight">Reset your password</h1><p className="mt-2 text-sm leading-6 text-slate-500">Enter your email and we’ll send reset instructions.</p></div>
    <Card className="border-slate-200 shadow-sm"><CardHeader className="border-b border-slate-100 px-6 py-5"><CardTitle className="text-base">Forgot password?</CardTitle><CardDescription>We’ll email a secure reset link if an account exists.</CardDescription></CardHeader><CardContent className="px-6 py-6">
      {sent ? <div className="space-y-4 text-center"><MailCheck className="mx-auto h-10 w-10 text-[#456b9a]" /><p className="text-sm leading-6 text-slate-600">If an account exists for <span className="font-medium">{email}</span>, password reset instructions have been sent.</p><Button variant="outline" asChild className="w-full"><Link to="/login">Return to sign in</Link></Button></div> : <form onSubmit={handleSubmit} className="space-y-5">{error && <div role="alert" className="rounded-lg border border-red-200 bg-red-50 px-3.5 py-3 text-sm text-red-700">{error}</div>}<AuthFormField id="email" label="Email" type="email" value={email} onChange={(event) => setEmail(event.target.value)} autoComplete="email" /><Button type="submit" disabled={isSubmitting} className="h-11 w-full bg-[#456b9a] hover:bg-[#3d608b]">{isSubmitting ? <><Loader2 className="mr-2 h-4 w-4 animate-spin" />Sending...</> : "Send reset link"}</Button></form>}
    </CardContent></Card>
    <Link to="/login" className="mt-6 flex items-center justify-center gap-2 text-sm font-medium text-[#456b9a] hover:underline"><ArrowLeft className="h-4 w-4" />Back to sign in</Link>
  </AuthPageLayout>;
}
