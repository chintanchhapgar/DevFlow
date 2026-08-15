import { useState, type ChangeEvent, type FormEvent } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import { ArrowRight, Loader2 } from "lucide-react";
import { register } from "@/features/auth/api/auth-api";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { AuthFormField } from "./AuthFormField";
import { AuthPageLayout } from "./AuthPageLayout";

function errorMessage(error: unknown, fallback: string) {
  return axios.isAxiosError(error) && error.response?.data?.message ? String(error.response.data.message) : fallback;
}

export function RegisterPage() {
  const navigate = useNavigate();
  const [form, setForm] = useState({ firstName: "", lastName: "", email: "", password: "", confirmPassword: "" });
  const [error, setError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const update = (key: keyof typeof form) => (event: ChangeEvent<HTMLInputElement>) => setForm({ ...form, [key]: event.target.value });

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (form.password !== form.confirmPassword) { setError("Passwords do not match."); return; }
    setError(""); setIsSubmitting(true);
    try {
      await register({ email: form.email, password: form.password, firstName: form.firstName, lastName: form.lastName });
      navigate("/login", { replace: true });
    } catch (caughtError) { setError(errorMessage(caughtError, "Unable to create your account.")); }
    finally { setIsSubmitting(false); }
  }

  return <AuthPageLayout>
    <div className="mb-8"><h1 className="text-3xl font-bold tracking-tight">Create your account</h1><p className="mt-2 text-sm leading-6 text-slate-500">Start organizing your team’s work with DevFlow.</p></div>
    <Card className="border-slate-200 shadow-sm"><CardHeader className="border-b border-slate-100 px-6 py-5"><CardTitle className="text-base">Sign up</CardTitle><CardDescription>All fields are required.</CardDescription></CardHeader><CardContent className="px-6 py-6"><form onSubmit={handleSubmit} className="space-y-4">
      {error && <div role="alert" className="rounded-lg border border-red-200 bg-red-50 px-3.5 py-3 text-sm text-red-700">{error}</div>}
      <div className="grid grid-cols-2 gap-4"><AuthFormField id="firstName" label="First name" value={form.firstName} onChange={update("firstName")} autoComplete="given-name" /><AuthFormField id="lastName" label="Last name" value={form.lastName} onChange={update("lastName")} autoComplete="family-name" /></div>
      <AuthFormField id="email" label="Email" type="email" value={form.email} onChange={update("email")} autoComplete="email" />
      <AuthFormField id="password" label="Password" type="password" value={form.password} onChange={update("password")} autoComplete="new-password" hint="At least 8 characters" />
      <AuthFormField id="confirmPassword" label="Confirm password" type="password" value={form.confirmPassword} onChange={update("confirmPassword")} autoComplete="new-password" />
      <Button type="submit" disabled={isSubmitting} className="h-11 w-full bg-[#456b9a] hover:bg-[#3d608b]">{isSubmitting ? <><Loader2 className="mr-2 h-4 w-4 animate-spin" />Creating account...</> : <>Create account <ArrowRight className="ml-2 h-4 w-4" /></>}</Button>
    </form></CardContent></Card>
    <p className="mt-6 text-center text-sm text-slate-500">Already have an account? <Link to="/login" className="font-medium text-[#456b9a] hover:underline">Sign in</Link></p>
  </AuthPageLayout>;
}
