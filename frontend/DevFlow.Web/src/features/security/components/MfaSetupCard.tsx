import { useEffect, useState } from "react";
import axios from "axios";
import { CheckCircle2, Copy, Loader2, ShieldCheck } from "lucide-react";
import { disableMfa, setupMfa, type MfaSetupResponse, verifyMfaSetup } from "@/features/auth/api/auth-api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useProfile } from "@/features/auth/hooks/use-profile";
import { useQueryClient } from "@tanstack/react-query";

export function MfaSetupCard() {
  const profileQuery = useProfile();
  const queryClient = useQueryClient();
  const [setup, setSetup] = useState<MfaSetupResponse | null>(null);
  const [code, setCode] = useState(""); const [recoveryCodes, setRecoveryCodes] = useState<string[] | null>(null);
  const [error, setError] = useState(""); const [busy, setBusy] = useState(false);
  const [isMfaEnabled, setIsMfaEnabled] = useState(false);
  const [isDisableOpen, setIsDisableOpen] = useState(false);
  const [disableCode, setDisableCode] = useState("");
  const [useRecoveryCode, setUseRecoveryCode] = useState(false);
  useEffect(() => {
    setIsMfaEnabled(profileQuery.data?.isTwoFactorEnabled === true);
  }, [profileQuery.data?.isTwoFactorEnabled]);
  const getError = (caughtError: unknown, fallback: string) => axios.isAxiosError(caughtError) && caughtError.response?.data?.message ? String(caughtError.response.data.message) : fallback;
  async function begin() { setError(""); setBusy(true); try { setSetup(await setupMfa()); } catch (caughtError) { setError(getError(caughtError, "Unable to start two-factor setup.")); } finally { setBusy(false); } }
  async function verify() { setError(""); setBusy(true); try { setRecoveryCodes(await verifyMfaSetup(code)); setIsMfaEnabled(true); await queryClient.invalidateQueries({ queryKey: ["auth", "profile"] }); } catch (caughtError) { setError(getError(caughtError, "The verification code is invalid.")); } finally { setBusy(false); } }
  async function disable() { setError(""); setBusy(true); try { await disableMfa(disableCode, useRecoveryCode); setIsMfaEnabled(false); setIsDisableOpen(false); setDisableCode(""); await queryClient.invalidateQueries({ queryKey: ["auth", "profile"] }); } catch (caughtError) { setError(getError(caughtError, "The verification code is invalid.")); } finally { setBusy(false); } }
  return <section className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm"><div className="flex items-start justify-between gap-4"><div className="flex gap-4"><div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-[#eef3f8]"><ShieldCheck className="h-5 w-5 text-[#456b9a]" /></div><div><h2 className="text-base font-semibold text-slate-900">Two-factor authentication</h2><p className="mt-1 text-sm leading-6 text-slate-500">{isMfaEnabled ? "Two-factor authentication is enabled for your account." : "Add an authenticator app code whenever you sign in."}</p></div></div>{isMfaEnabled ? <Button type="button" variant="outline" onClick={() => setIsDisableOpen((open) => !open)} disabled={busy} className="border-red-200 text-red-700 hover:bg-red-50 hover:text-red-800">Disable MFA</Button> : !setup && !recoveryCodes && <Button onClick={begin} disabled={busy} className="bg-[#456b9a] hover:bg-[#3d608b]">{busy && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}Set up MFA</Button>}</div>
    {error && <p role="alert" className="mt-4 rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">{error}</p>}
    {isMfaEnabled && isDisableOpen && <form onSubmit={(event) => { event.preventDefault(); void disable(); }} className="mt-6 rounded-xl border border-red-200 bg-red-50 p-4"><p className="text-sm font-semibold text-red-900">Disable two-factor authentication?</p><p className="mt-1 text-sm text-red-700">Enter a current authenticator code or a recovery code to confirm.</p><label className="mt-4 flex items-center gap-2 text-sm text-red-800"><input type="checkbox" checked={useRecoveryCode} onChange={(event) => setUseRecoveryCode(event.target.checked)} /> Use a recovery code</label><div className="mt-3 flex flex-col gap-3 sm:flex-row"><Input value={disableCode} onChange={(event) => setDisableCode(event.target.value)} inputMode={useRecoveryCode ? "text" : "numeric"} maxLength={32} placeholder={useRecoveryCode ? "Recovery code" : "123456"} required className="border-red-200 bg-white" /><Button type="submit" variant="destructive" disabled={busy || !disableCode.trim()}>{busy && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}Disable MFA</Button></div></form>}
    {setup && !recoveryCodes && <div className="mt-6 border-t border-slate-100 pt-6"><p className="text-sm font-medium text-slate-800">1. Scan this QR code with your authenticator app</p><img src={setup.qrCodeImage} alt="Scan to add DevFlow to your authenticator app" className="mt-4 h-40 w-40 rounded-lg border border-slate-200 p-2" /><p className="mt-4 text-sm font-medium text-slate-800">2. Or enter this key manually</p><div className="mt-2 flex gap-2"><code className="min-w-0 flex-1 break-all rounded-md bg-slate-100 px-3 py-2 text-xs text-slate-700">{setup.manualEntryKey}</code><Button type="button" variant="outline" size="icon" aria-label="Copy setup key" onClick={() => void navigator.clipboard.writeText(setup.manualEntryKey)}><Copy className="h-4 w-4" /></Button></div><p className="mt-5 text-sm font-medium text-slate-800">3. Enter the six-digit code</p><div className="mt-2 flex gap-3"><Input value={code} onChange={(event) => setCode(event.target.value.replace(/\D/g, ""))} inputMode="numeric" maxLength={6} placeholder="123456" className="h-10 max-w-40 border-[#cbd5e1] bg-white text-slate-900" /><Button onClick={verify} disabled={busy || code.length !== 6} className="bg-[#456b9a] hover:bg-[#3d608b]">{busy && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}Enable MFA</Button></div></div>}
    {recoveryCodes && <div className="mt-6 rounded-xl border border-emerald-200 bg-emerald-50 p-4"><div className="flex gap-2 text-emerald-800"><CheckCircle2 className="h-5 w-5 shrink-0" /><div><p className="font-medium">Two-factor authentication is enabled.</p><p className="mt-1 text-sm">Save these recovery codes somewhere safe. They are shown only once.</p></div></div><div className="mt-4 grid grid-cols-2 gap-2 rounded-lg bg-white p-3 font-mono text-sm text-slate-700">{recoveryCodes.map((recoveryCode) => <span key={recoveryCode}>{recoveryCode}</span>)}</div></div>}
  </section>;
}
