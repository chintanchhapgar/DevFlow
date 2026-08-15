import { useEffect, useRef, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import axios from "axios";
import { CheckCircle2, CircleX, Loader2 } from "lucide-react";
import { verifyEmail } from "@/features/auth/api/auth-api";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { AuthPageLayout } from "./AuthPageLayout";

export function VerifyEmailPage() {
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token");
  const verificationRequest = useRef<{
    token: string;
    promise: Promise<void>;
  } | null>(null);
  const [status, setStatus] = useState<"verifying" | "success" | "error">(token ? "verifying" : "error");
  const [error, setError] = useState(token ? "" : "This verification link is invalid or incomplete.");

  useEffect(() => {
    if (!token) return;

    if (verificationRequest.current?.token !== token) {
      verificationRequest.current = {
        token,
        promise: verifyEmail(token),
      };
    }

    let active = true;
    void verificationRequest.current.promise
      .then(() => { if (active) setStatus("success"); })
      .catch((caughtError: unknown) => {
        if (!active) return;
        setError(axios.isAxiosError(caughtError) && caughtError.response?.data?.message ? String(caughtError.response.data.message) : "We could not verify your email. The link may have expired.");
        setStatus("error");
      });
    return () => { active = false; };
  }, [token]);

  const isSuccess = status === "success";
  return <AuthPageLayout>
    <div className="mb-8"><h1 className="text-3xl font-bold tracking-tight">Email verification</h1><p className="mt-2 text-sm leading-6 text-slate-500">We’re confirming your DevFlow email address.</p></div>
    <Card className="border-slate-200 shadow-sm"><CardHeader className="border-b border-slate-100 px-6 py-5"><CardTitle className="text-base">{isSuccess ? "Email verified" : status === "verifying" ? "Verifying your email" : "Verification unsuccessful"}</CardTitle><CardDescription>{isSuccess ? "Your account is ready to use." : "Email verification helps keep your account secure."}</CardDescription></CardHeader><CardContent className="px-6 py-7 text-center">
      {status === "verifying" && <><Loader2 className="mx-auto h-10 w-10 animate-spin text-[#456b9a]" /><p className="mt-4 text-sm text-slate-600">Please wait while we verify your email.</p></>}
      {isSuccess && <><CheckCircle2 className="mx-auto h-10 w-10 text-emerald-600" /><p className="mt-4 text-sm leading-6 text-slate-600">Your email has been verified successfully. You can now sign in.</p><Button asChild className="mt-6 w-full bg-[#456b9a] hover:bg-[#3d608b]"><Link to="/login">Sign in</Link></Button></>}
      {status === "error" && <><CircleX className="mx-auto h-10 w-10 text-red-600" /><p role="alert" className="mt-4 text-sm leading-6 text-red-700">{error}</p><Button variant="outline" asChild className="mt-6 w-full"><Link to="/login">Back to sign in</Link></Button></>}
    </CardContent></Card>
  </AuthPageLayout>;
}
